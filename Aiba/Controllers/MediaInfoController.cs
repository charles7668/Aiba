using Aiba.MediaInfoProviders;
using Aiba.Model;
using Aiba.Model.RequestParams;
using Aiba.Plugin;
using Aiba.Plugin.Scanner;
using Aiba.Repository;
using Aiba.Scanners;
using Aiba.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace Aiba.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediaInfoController(
        MediaProviderFactory mediaProviderFactory,
        ILogger<MediaInfoController> logger,
        IUnitOfWork unitOfWork,
        UserManager<IdentityUser> userManager)
        : Controller
    {
        private const int ITEMS_PER_PAGE = 20;

        [HttpGet("detail/{providerName}")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<MediaInfo>> GetDetail(string providerName, [FromQuery] string url,
            [FromQuery(Name = "library")] string? libraryName)
        {
            string decodeProviderName = HttpUtility.UrlDecode(providerName);
            logger.LogInformation("MediaInfoController.GetDetail called with providerName: {ProviderName}",
                decodeProviderName);
            string decodeUrl = HttpUtility.UrlDecode(url);

            // use local file data
            if (providerName.ToLower() == "local")
            {
                string? userId = userManager.GetUserId(User);
                if (userId == null)
                {
                    return Unauthorized();
                }

                try
                {
                    libraryName ??= "";
                    MediaInfo? mediaInfo = await unitOfWork.GetMediaInfoAsync(userId, libraryName,
                        decodeUrl.StartsWith("file://") ? decodeUrl : "file://" + decodeUrl);
                    if (mediaInfo == null)
                    {
                        logger.LogWarning("MediaInfo not found");
                        return NotFound();
                    }

                    return mediaInfo;
                }
                catch (Exception e)
                {
                    logger.LogError("GetDetail failed : {Exception}", e.ToString());
                    return NotFound();
                }
            }

            IMediaInfoProvider? provider = mediaProviderFactory.GetProvider(decodeProviderName);
            if (provider == null)
            {
                logger.LogError("Provider not found");
                return BadRequest("Provider not found");
            }

            MediaInfo detailInfo = await provider.GetDetailInfoAsync(decodeUrl, CancellationToken.None);
            return Ok(detailInfo);
        }

        [HttpPost]
        public async Task<ActionResult> AddMediaInfoToLibrary(AddMediaInfoRequest request)
        {
            string? userId = userManager.GetUserId(User);
            logger.LogInformation("MediaInfoController.AddMediaInfoToLibrary called with userId: {UserId}", userId);
            if (userId == null)
            {
                return Unauthorized();
            }

            try
            {
                await unitOfWork.AddMediaInfoToLibraryAsync(userId, request.LibraryInfo.Name, request.MediaInfo);
            }
            catch (Exception e)
            {
                logger.LogError("AddMediaInfoToLibrary failed : {Exception}", e.ToString());
                return BadRequest($"AddMediaInfoToLibrary failed : {e.Message}");
            }

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveMediaInfoFromLibrary(RemoveMediaInfoRequest request)
        {
            string? userId = userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized();
            }

            logger.LogInformation("MediaInfoController.RemoveMediaInfoFromLibrary called with userId: {UserId}",
                userId);

            await unitOfWork.RemoveMediaInfo(userId, request.LibraryInfo.Name, request.MediaInfo);
            if (!request.MediaInfo.Url.StartsWith("file://"))
                return Ok();
            IAppPathService appPathService = HttpContext.RequestServices.GetRequiredService<IAppPathService>();
            string coverPath = request.MediaInfo.ImageUrl[7..];
            if (!coverPath.StartsWith(appPathService.CoverPath))
                return Ok();
            try
            {
                System.IO.File.Delete(coverPath);
            }
            catch
            {
                // ignore
            }

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MediaInfo>>> GetMediaInfosFromLibrary([FromQuery] string libraryName,
            [FromQuery(Name = "page")] string pageString = "")
        {
            bool parseResult = int.TryParse(pageString, out int page);
            if (!parseResult && string.IsNullOrEmpty(pageString))
            {
                logger.LogWarning("Page param is not number, use default value 1");
                page = 1;
            }

            string? userId = userManager.GetUserId(User);
            logger.LogInformation("MediaInfoController.GetMediaInfosFromLibrary called by userId: {UserId}", userId);
            if (userId == null)
            {
                return Unauthorized();
            }

            LibraryInfo libraryInfo = new()
            {
                Name = libraryName
            };

            IEnumerable<MediaInfo> mediaInfos;
            logger.LogInformation("Query page {Page} from library {LibraryName}", page, libraryName);
            try
            {
                mediaInfos = await unitOfWork.GetMediaInfos(userId, libraryInfo.Name, page, ITEMS_PER_PAGE);
            }
            catch (Exception e)
            {
                logger.LogError("GetMediaInfosFromLibrary failed : {Exception}", e.ToString());
                return BadRequest($"GetMediaInfosFromLibrary failed : {e.Message}");
            }

            return Ok(mediaInfos);
        }

        [HttpGet("ImageLinks/{providerName}")]
        public async Task<ActionResult<IEnumerable<string>>> GetImageLinks(string providerName,
            [FromQuery(Name = "library")] string library, [FromQuery(Name = "url")] string mediaUrl,
            [FromQuery(Name = "chapter")] string? chapterName = null)
        {
            string userId = userManager.GetUserId(User)!;
            string decodeProviderName = HttpUtility.UrlDecode(providerName);
            string decodeLibrary = HttpUtility.UrlDecode(library);
            string decodeMediaUrl = HttpUtility.UrlDecode(mediaUrl);
            string decodeChapterName = HttpUtility.UrlDecode(chapterName ?? "");
            if (decodeProviderName.ToLower() == "local")
            {
                LibraryInfo? libraryInfo = await unitOfWork.GetLibraryInfo(userId, decodeLibrary);
                MediaInfo? mediaInfo = await unitOfWork.GetMediaInfoAsync(userId, decodeLibrary, decodeMediaUrl);
                IScanner? scanner = Program.ServiceProvider.GetRequiredService<ScannerFactory>()
                    .GetScanner(libraryInfo?.ScannerName ?? "");
                if (mediaInfo == null || libraryInfo == null || scanner == null)
                {
                    return NotFound();
                }

                IEnumerable<string> result = await scanner.GetMediaListAsync(libraryInfo.Path, mediaInfo.Url,
                    decodeChapterName,
                    CancellationToken.None);
                return Ok(result);
            }

            // todo from remote provider
            return Ok(Array.Empty<MediaInfo>());
        }
    }
}