using Aiba.Model;
using Aiba.Model.Constants;
using Aiba.Plugin;

namespace Aiba.MediaInfoProviders
{
    public class ShenShiMangaInfoProvider : IMediaInfoProvider
    {
        public string Name => "ShenShiManga";

        public Task<IEnumerable<MediaInfo>> SearchAsync(string type, string searchText,
            CancellationToken cancellationToken)
        {
            if (type != MediaInfoType.MANGA)
                return Task.FromResult(new List<MediaInfo>().AsEnumerable());
            return Task.FromResult(new List<MediaInfo>().AsEnumerable());
        }
    }
}