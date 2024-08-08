﻿using Aiba.Model;
using Aiba.Plugin.Scanner;
using Aiba.Repository;
using Aiba.Scanners;
using System.Runtime.CompilerServices;
using Exception = System.Exception;

namespace Aiba.TaskManager
{
    public static class TaskExecutor
    {
        public static void ScanningTask(string userId, LibraryInfo libraryInfo,
            string scannerName,
            CancellationToken cancellationToken)
        {
            ILogger logger = Program.ServiceProvider.GetRequiredService<ILogger>();
            logger.LogInformation("Scanning media infos by user {User} , Library : {LibraryName}", userId,
                libraryInfo.Name);
            IServiceProvider scope = Program.ServiceProvider.CreateAsyncScope().ServiceProvider;
            ScannerFactory scannerFactory = scope.GetRequiredService<ScannerFactory>();
            IUnitOfWork unitOfWork = scope.GetRequiredService<IUnitOfWork>();
            IScanner? scanner = scannerFactory.GetScanner(scannerName);
            if (scanner == null)
            {
                return;
            }

            scanner.ScanAsync(libraryInfo.Path, mediaInfo =>
            {
                try
                {
                    ConfiguredTaskAwaitable temp = unitOfWork
                        .AddMediaInfoToLibraryAsync(userId, libraryInfo, mediaInfo)
                        .ConfigureAwait(false);
                    temp.GetAwaiter().GetResult();
                }
                catch (Exception e)
                {
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