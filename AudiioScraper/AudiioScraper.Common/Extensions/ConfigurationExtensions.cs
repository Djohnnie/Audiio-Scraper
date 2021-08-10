using Microsoft.Extensions.Configuration;

namespace AudiioScraper.Worker.Extensions
{
    public static class ConfigurationExtensions
    {
        const string DB_CONNECTION_STRING = nameof(DB_CONNECTION_STRING);
        const string MUSIC_DOWNLOAD_PATH = nameof(MUSIC_DOWNLOAD_PATH);

        public static string GetConnectionString(this IConfiguration configuration)
        {
            return configuration.GetValue<string>(DB_CONNECTION_STRING);
        }

        public static string GetMusicDownloadPath(this IConfiguration configuration)
        {
            return configuration.GetValue<string>(DB_CONNECTION_STRING);
        }
    }
}