using Aiba.Entities;
using Aiba.MediaInfoProviders;
using Aiba.Model;
using Aiba.Plugin;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace Aiba.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediaInfoController : Controller
    {
        public MediaInfoController(MediaProviderFactory mediaProviderFactory, ILogger<MediaInfoController> logger,
            AppDBContext context)
        {
            _mediaProviderFactory = mediaProviderFactory;
            _logger = logger;
            _context = context;
        }

        private readonly AppDBContext _context;

        private readonly ILogger<MediaInfoController> _logger;

        private readonly MediaProviderFactory _mediaProviderFactory;

        [HttpGet("detail/{providerName}")]
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
    }
}