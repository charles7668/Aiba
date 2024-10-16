using Aiba.Extensions;
using Aiba.Model;
using Aiba.Plugin.Scanner;
using Aiba.Repository;
using Aiba.Scanners;
using Aiba.Services;
using SkiaSharp;
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
                        const int targetWidth = 500;
                        if (mediaInfo.ImageUrl.StartsWith("file://"))
                        {
                            SKEncodedImageFormat format;
                            SKBitmap originImage;
                            await using (FileStream fileStream = File.OpenRead(mediaInfo.ImageUrl[7..]))
                            {
                                format = ImageService.GetImageFormat(fileStream) ??
                                         SKEncodedImageFormat.Jpeg;
                            }

                            await using (FileStream fileStream = File.OpenRead(mediaInfo.ImageUrl[7..]))
                            {
                                originImage = SKBitmap.Decode(fileStream);
                            }

                            var originImageSize = new SKSize(originImage.Width, originImage.Height);


                            await using (FileStream fileStream = File.OpenRead(mediaInfo.ImageUrl[7..]))
                            {
                                SKBitmap resizedImage = ImageService.Resize(fileStream, targetWidth,
                                    (int)(originImageSize.Height * (targetWidth / originImageSize.Width)));
                                ImageService.SaveBitmapToFile(resizedImage, coverPath, format);
                            }

                            mediaInfo.ImageUrl = $"file://{coverPath}";
                        }
                        else if (!mediaInfo.ImageUrl.IsHttpLink())
                        {
                            byte[] bytes = Convert.FromBase64String(mediaInfo.ImageUrl);
                            SKEncodedImageFormat format;
                            SKBitmap originImage;
                            await using (Stream fileStream = new MemoryStream(bytes))
                            {
                                format = ImageService.GetImageFormat(fileStream) ??
                                         SKEncodedImageFormat.Jpeg;
                            }

                            await using (Stream fileStream = new MemoryStream(bytes))
                            {
                                originImage = SKBitmap.Decode(fileStream);
                            }

                            var originImageSize = new SKSize(originImage.Width, originImage.Height);

                            await using (Stream fileStream = new MemoryStream(bytes))
                            {
                                SKBitmap resizedImage = ImageService.Resize(fileStream, targetWidth,
                                    (int)(originImageSize.Height * (targetWidth / originImageSize.Width)));
                                ImageService.SaveBitmapToFile(resizedImage, coverPath, format);
                            }

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