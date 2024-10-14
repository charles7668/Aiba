using Aiba.Extensions;
using Aiba.Model;
using Aiba.Plugin.Scanner;
using Aiba.Repository;
using Aiba.Scanners;
using Aiba.Services;
using Exception = System.Exception;

namespace Aiba.TaskManager
{
    public static class TaskExecutor
    {
        public static void ScanningTask(string userId, LibraryInfo libraryInfo,
            string scannerName,
            CancellationToken cancellationToken)
        {
            ILoggerFactory loggerFactory = Program.ServiceProvider.GetRequiredService<ILoggerFactory>();
            ILogger logger = loggerFactory.CreateLogger("TaskExecutor");
            logger.LogInformation("Scanning media infos by user {User} , Library : {LibraryName} start", userId,
                libraryInfo.Name);
            IServiceProvider scope = Program.ServiceProvider.CreateAsyncScope().ServiceProvider;
            ScannerFactory scannerFactory = scope.GetRequiredService<ScannerFactory>();
            IUnitOfWork unitOfWork = scope.GetRequiredService<IUnitOfWork>();
            IScanner? scanner = scannerFactory.GetScanner(scannerName);
            IAppPathService appPathService = scope.GetRequiredService<IAppPathService>();
            if (scanner == null)
            {
                return;
            }

            scanner.ScanAsync(libraryInfo.Path, async callbackParam =>
            {
                MediaInfo mediaInfo = callbackParam.MediaInfo;
                string coverExt = callbackParam.CoverExt;
                if (string.IsNullOrWhiteSpace(coverExt))
                    coverExt = ".jpg";
                else if (coverExt[0] != '.')
                    coverExt = "." + coverExt;
                string coverGuid = Guid.NewGuid().ToString();
                string coverPath = Path.Combine(appPathService.CoverPath, $"{coverGuid}{coverExt}");
                try
                {
                    MediaInfo? info = await unitOfWork.GetMediaInfoAsync(userId, libraryInfo.Name, mediaInfo.Url);
                    if (info == null)
                    {
                        if (mediaInfo.ImageUrl.StartsWith("file://"))
                        {
                            File.Copy(mediaInfo.ImageUrl[7..], coverPath);
                            mediaInfo.ImageUrl = $"file://{coverPath}";
                        }
                        else if (!mediaInfo.ImageUrl.IsHttpLink())
                        {
                            byte[] bytes = Convert.FromBase64String(mediaInfo.ImageUrl);
                            await File.WriteAllBytesAsync(coverPath, bytes, cancellationToken);
                            mediaInfo.ImageUrl = $"file://{coverPath}";
                        }

                        await unitOfWork.AddMediaInfoToLibraryAsync(userId, libraryInfo.Name, mediaInfo);
                    }
                }
                catch (Exception e)
                {
                    if (!File.Exists(coverPath))
                        return Result.Failure(e.Message);
                    try
                    {
                        File.Delete(coverPath);
                    }
                    catch
                    {
                        // ignore
                    }

                    // ignore
                    return Result.Failure(e.Message);
                }

                return Result.Success();
            }, cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();
            logger.LogInformation("Scanning media infos by user {User} , Library : {LibraryName} finished", userId,
                libraryInfo.Name);
        }
    }
}