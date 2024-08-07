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
    public class MediaInfoController : Controller
    {
        public MediaInfoController(MediaProviderFactory mediaProviderFactory, ILogger<MediaInfoController> logger,
            IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _mediaProviderFactory = mediaProviderFactory;
            _logger = logger;
            _unitOfWord = unitOfWork;
            _userManager = userManager;
        }


        private readonly ILogger<MediaInfoController> _logger;

        private readonly MediaProviderFactory _mediaProviderFactory;
        private readonly IUnitOfWork _unitOfWord;
        private readonly UserManager<IdentityUser> _userManager;

        [HttpGet("detail/{providerName}")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<MediaInfo>> GetDetail(string providerName, [FromQuery] string url)
        {
            string decodeProviderName = HttpUtility.UrlDecode(providerName);
            _logger.LogInformation("MediaInfoController.GetDetail called with providerName: {providerName}",
                decodeProviderName);
            IMediaInfoProvider? provider = _mediaProviderFactory.GetProvider(decodeProviderName);
            if (provider == null)
            {
                _logger.LogError("Provider not found");
                return BadRequest("Provider not found");
            }

            string decodeUrl = HttpUtility.UrlDecode(url);

            MediaInfo detailInfo = await provider.GetDetailInfoAsync(decodeUrl, CancellationToken.None);
            return Ok(detailInfo);
        }

        [HttpPost]
        public async Task<ActionResult> AddMediaInfoToLibrary(AddMediaInfoRequest request)
        {
            string? userId = _userManager.GetUserId(User);
            _logger.LogInformation("MediaInfoController.AddMediaInfoToLibrary called with userId: {userId}", userId);
            if (userId == null)
            {
                return Unauthorized();
            }

            try
            {
                await _unitOfWord.AddMediaInfoToLibraryAsync(userId, request.LibraryInfo, request.MediaInfo);
            }
            catch (Exception e)
            {
                _logger.LogError("AddMediaInfoToLibrary failed : {Exception}", e.ToString());
                return BadRequest($"AddMediaInfoToLibrary failed : {e.Message}");
            }

            return Ok();
        }

        [HttpGet]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        public async Task<ActionResult<IEnumerable<MediaInfo>>> GetMediaInfosFromLibrary([FromQuery] string libraryName)
        {
            string? userId = _userManager.GetUserId(User);
            _logger.LogInformation("MediaInfoController.GetMediaInfosFromLibrary called by userId: {userId}", userId);
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
                mediaInfos = await _unitOfWord.GetMediaInfosFromLibrary(userId, libraryInfo);
            }
            catch (Exception e)
            {
                _logger.LogError("GetMediaInfosFromLibrary failed : {Exception}", e.ToString());
                return BadRequest($"GetMediaInfosFromLibrary failed : {e.Message}");
            }

            return Ok(mediaInfos);
        }
    }
}