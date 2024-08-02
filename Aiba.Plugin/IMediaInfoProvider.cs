using Aiba.Enums;
using Aiba.Model;

namespace Aiba.Plugin
{
    public interface IMediaInfoProvider
    {
        public string Name { get; }

        public Task<IEnumerable<MediaInfo>> SearchAsync(MediaTypeFlag type, string searchText,
            CancellationToken cancellationToken);
    }
}