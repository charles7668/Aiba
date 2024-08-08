using Aiba.MediaInfoProviders;
using Aiba.Plugin;
using Aiba.Plugin.Scanner;
using Aiba.Scanners;
using Aiba.Services;
using Aiba.TaskManager;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Aiba.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddMediaInfoProviders(this IServiceCollection services)
        {
            services.AddSingleton<IMediaInfoProvider, ShenShiMangaInfoProvider>();
            services.TryAddSingleton<MediaProviderFactory, MediaProviderFactory>();
        }

        public static void AddDecompressServices(this IServiceCollection services)
        {
            services.AddSingleton<IDecompressService, GzipDecompressService>();
            services.AddSingleton<IDecompressService, DeflateDecompressService>();
            services.AddSingleton<IDecompressService, BrotliDecompressService>();
            services.AddSingleton<IDecompressService, DefaultDecompressService>();
            services.TryAddSingleton<DecompressServiceFactory, DecompressServiceFactory>();
        }

        public static void AddScanners(this IServiceCollection services)
        {
            services.AddSingleton<IScanner, MangaFolderStructureScanner>();
            services.TryAddSingleton<ScannerFactory, ScannerFactory>();
        }

        public static void AddTaskManager(this IServiceCollection services)
        {
            services.AddSingleton<ITaskManager, TaskManager.TaskManager>();
        }
    }
}