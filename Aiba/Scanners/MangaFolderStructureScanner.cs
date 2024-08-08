using Aiba.Model;
using Aiba.Plugin.Scanner;

namespace Aiba.Scanners
{
    public class MangaFolderStructureScanner : IScanner
    {
        public static readonly HashSet<string> SupportImageExtension = new()
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".bmp",
            ".gif"
        };

        public string Name { get; } = "MangaFolderStructureScanner";

        public Task<Result> ScanAsync(string libraryRootPath, Func<MediaInfo, Result> callback,
            CancellationToken cancellationToken)
        {
            var scanningQueue = new Queue<string>();
            scanningQueue.Enqueue(libraryRootPath);
            while (scanningQueue.Count > 0 && !cancellationToken.IsCancellationRequested)
            {
                string path = scanningQueue.Dequeue();
                string? firstImage = Directory.EnumerateFiles(path)
                    .FirstOrDefault(x => SupportImageExtension.Contains(Path.GetExtension(x.ToLower())));
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
                else
                {
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
            }

            return Task.FromResult(Result.Success());
        }
    }
}