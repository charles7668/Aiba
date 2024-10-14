using Aiba.Enums;
using Aiba.Model;

namespace Aiba.Plugin.Scanner
{
    public interface IScanner
    {
        public MediaTypeFlag SupportedMediaType { get; }
        public string Name { get; }

        public Task<Result> ScanAsync(string libraryRootPath, Func<MediaInfo, Result> callback,
            CancellationToken cancellationToken);

        public Task<IEnumerable<string>> GetMediaListAsync(string libraryRootPath, string mediaUrl,
            string? chapterName,
            CancellationToken cancellationToken);
    }
}