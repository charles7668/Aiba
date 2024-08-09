using Aiba.MediaInfoProviders;
using Aiba.Model;
using Aiba.Model.RequestParams;
using Aiba.Plugin;
using Aiba.Repository;
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
                    MediaInfo? mediaInfo = await unitOfWork.GetMediaInfo(userId, libraryName,
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MediaInfo>>> GetMediaInfosFromLibrary([FromQuery] string libraryName)
        {
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
            try
            {
                mediaInfos = await unitOfWork.GetMediaInfosFromLibraryName(userId, libraryInfo.Name);
            }
            catch (Exception e)
            {
                logger.LogError("GetMediaInfosFromLibrary failed : {Exception}", e.ToString());
                return BadRequest($"GetMediaInfosFromLibrary failed : {e.Message}");
            }

            return Ok(mediaInfos);
        }
    }
}