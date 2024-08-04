using Aiba.Enums;
using Aiba.Model;

namespace Aiba.Plugin
{
    public interface IMediaInfoProvider
    {
        public string ProviderName { get; }

        public string ProviderUrl { get; }

        public Task<IEnumerable<MediaInfo>> SearchAsync(MediaTypeFlag type, string searchText,
            int page,
            CancellationToken cancellationToken);

        public Task<MediaInfo> GetDetailInfoAsync(string url, CancellationToken cancellationToken);
    }
}