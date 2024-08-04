using Aiba.Enums;
using Aiba.Extensions;
using Aiba.MediaInfoProviders;
using Aiba.Model;
using Aiba.Plugin;
using Microsoft.AspNetCore.Mvc;
using SearchOption = Aiba.Model.Requests.SearchOption;

namespace Aiba.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        public SearchController(ILogger<SearchController> logger, MediaProviderFactory providerFactory)
        {
            _logger = logger;
            _mediaProviderFactory = providerFactory;
        }

        private readonly ILogger<SearchController> _logger;

        private readonly MediaProviderFactory _mediaProviderFactory;

        [HttpPost]
        [Produces("application/json")]
        public ActionResult<IEnumerable<MediaInfo>> Post([FromBody] SearchOption searchOption)
        {
            string type = searchOption.SearchType;
            _logger.LogInformation("SearchController.Get called with type: {type}", type);
            MediaTypeFlag flag = type.GetFlag();
            IEnumerable<IMediaInfoProvider> providers = _mediaProviderFactory.Providers;
            List<Task> tasks = [];
            List<MediaInfo> result = [];
            try
            {
                foreach (IMediaInfoProvider mediaInfoProvider in providers)
                {
                    Task<IEnumerable<MediaInfo>> task =
                        mediaInfoProvider.SearchAsync(flag, searchOption.SearchText, searchOption.Page,
                            CancellationToken.None);
                    task.ContinueWith(async t =>
                    {
                        IEnumerable<MediaInfo> infos = await t;
                        result.AddRange(infos);
                    });
                    tasks.Add(task);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("search failed : {Exception}", e.ToString());
                return StatusCode(500, e.Message);
            }

            Task.WaitAll(tasks.ToArray());

            return result;
        }
    }
}