using Aiba.Enums;
using Aiba.Extensions;
using Aiba.Model;
using Microsoft.AspNetCore.Mvc;
using SearchOption = Aiba.Model.Requests.SearchOption;

namespace Aiba.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        public SearchController(ILogger<SearchController> logger)
        {
            _logger = logger;
        }

        private readonly ILogger<SearchController> _logger;

        [HttpPost]
        [Produces("application/json")]
        public ActionResult<IEnumerable<MediaInfo>> Post([FromBody] SearchOption searchOption)
        {
            string type = searchOption.SearchType;
            _logger.LogInformation("SearchController.Get called with type: {type}", type);
            MediaTypeFlag flag = type.GetFlag();
            if (flag.HasFlag(MediaTypeFlag.MANGA))
                return new List<MediaInfo>();
            _logger.LogError("invalidArgument with type : {SearchType}", type);
            return BadRequest($"invalidArgument with type : {type}");
        }
    }
}