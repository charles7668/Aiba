namespace Aiba.Services
{
    public class AppPathService : IAppPathService
    {
        public AppPathService()
        {
            string? configPath = Environment.GetEnvironmentVariable("AIBA_CONFIG_PATH");
            if (string.IsNullOrWhiteSpace(configPath))
            {
#if DEBUG
                configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Aiba");
#else
                configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Aiba");
#endif
            }

            CoverPath = Path.Combine(configPath, "Covers");

            Directory.CreateDirectory(CoverPath);
        }

        public string CoverPath { get; }
    }
}