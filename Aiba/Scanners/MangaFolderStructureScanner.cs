using Aiba.Enums;
using Aiba.Helpers;
using Aiba.Model;
using Aiba.Plugin.Scanner;

namespace Aiba.Scanners
{
    public class MangaFolderStructureScanner : IScanner
    {
        private static readonly HashSet<string> _SupportImageExtension =
        [
            ".jpg",
            ".jpeg",
            ".png",
            ".bmp",
            ".gif",
            ".webp"
        ];

        public MediaTypeFlag SupportedMediaType => MediaTypeFlag.MANGA;
        public string Name => "MangaFolderStructureScanner";

        public Task<Result> ScanAsync(string libraryRootPath, Func<MediaInfo, Result> callback,
            CancellationToken cancellationToken)
        {
            var scanningQueue = new Queue<string>();
            scanningQueue.Enqueue(libraryRootPath);
            while (scanningQueue.Count > 0 && !cancellationToken.IsCancellationRequested)
            {
                string path = scanningQueue.Dequeue();
                string? firstImage = Directory.EnumerateFiles(path)
                    .FirstOrDefault(x => _SupportImageExtension.Contains(Path.GetExtension(x.ToLower())));
                if (firstImage != null)
                {
                    var mediaInfo = new MediaInfo
                    {
                        Name = Path.GetFileName(path), // directory name as media name
                        Url = "file://" + path,
                        ImageUrl = "file://" + firstImage,
                        ProviderName = "local",
                        Type = "manga"
                    };
                    callback(mediaInfo);
                }

                IEnumerable<string> cbzFiles =
                    Directory.EnumerateFiles(path).Where(x => x.ToLower().EndsWith(".cbz"));
                foreach (string cbzFile in cbzFiles)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;
                    string cbzFullPath = Path.GetFullPath(cbzFile);
                    var mediaInfo = new MediaInfo
                    {
                        Name = Path.GetFileNameWithoutExtension(cbzFile),
                        Url = "file://" + cbzFullPath,
                        ImageUrl = "file://" + cbzFullPath,
                        ProviderName = "local",
                        Type = "manga"
                    };
                    callback(mediaInfo);
                }

                IEnumerable<string> directories = Directory.EnumerateDirectories(path);
                foreach (string directory in directories)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;
                    scanningQueue.Enqueue(directory);
                }
            }

            return Task.FromResult(Result.Success());
        }

        public Task<IEnumerable<string>> GetMediaListAsync(string libraryRootPath, string mediaUrl,
            string? chapterName,
            CancellationToken cancellationToken)
        {
            var directoryInfo = new DirectoryInfo(mediaUrl.TrimFileProtocol());
            if (!directoryInfo.Exists)
            {
                return Task.FromResult<IEnumerable<string>>([]);
            }

            IEnumerable<string> files = directoryInfo.EnumerateFiles()
                .Where(f => _SupportImageExtension.Contains(f.Extension.ToLower()))
                .Select(f => f.FullName.ToFileProtocol());
            return Task.FromResult(files);
        }
    }
}