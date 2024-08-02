using Aiba.Model;

namespace Aiba.Plugin
{
    public interface IMediaInfoProvider
    {
        public string Name { get; }

        public Task<IEnumerable<MediaInfo>>
            SearchAsync(string type, string searchText, CancellationToken cancellationToken);
    }
}