using Aiba.Model;

namespace Aiba.Plugin.Scanner
{
    public interface IScanner
    {
        public string Name { get; }
        public Task<Result> ScanAsync(string libraryRootPath, Func<MediaInfo, Result> callback,
            CancellationToken cancellationToken);
    }
}