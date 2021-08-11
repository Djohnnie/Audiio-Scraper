using Microsoft.Extensions.Configuration;

namespace AudiioScraper.Common.Extensions
{
    public static class ConfigurationExtensions
    {
        const string DB_CONNECTION_STRING = nameof(DB_CONNECTION_STRING);
        const string SCRAPER_INTERVAL_IN_SECONDS = nameof(SCRAPER_INTERVAL_IN_SECONDS);
        const string DOWNLOADER_INTERVAL_IN_SECONDS = nameof(DOWNLOADER_INTERVAL_IN_SECONDS);
        const string MUSIC_DOWNLOAD_PATH = nameof(MUSIC_DOWNLOAD_PATH);
        const string SFX_DOWNLOAD_PATH = nameof(SFX_DOWNLOAD_PATH);
        const string MUSIC_API_ADDRESS = nameof(MUSIC_API_ADDRESS);
        const string SFX_API_ADDRESS = nameof(SFX_API_ADDRESS);
        const string MUSIC_CDN_BASE_ADDRESS = nameof(MUSIC_CDN_BASE_ADDRESS);
        const string SFX_CDN_BASE_ADDRESS = nameof(SFX_CDN_BASE_ADDRESS);
        const string ART_CDN_BASE_ADDRESS = nameof(ART_CDN_BASE_ADDRESS);

        public static string GetConnectionString(this IConfiguration configuration)
        {
            return configuration.GetValue<string>(DB_CONNECTION_STRING);
        }

        public static int GetScraperIntervalInSeconds(this IConfiguration configuration)
        {
            return configuration.GetValue<int>(SCRAPER_INTERVAL_IN_SECONDS);
        }

        public static int GetDownloaderIntervalInSeconds(this IConfiguration configuration)
        {
            return configuration.GetValue<int>(DOWNLOADER_INTERVAL_IN_SECONDS);
        }

        public static string GetMusicDownloadPath(this IConfiguration configuration)
        {
            return configuration.GetValue<string>(MUSIC_DOWNLOAD_PATH);
        }

        public static string GetSfxDownloadPath(this IConfiguration configuration)
        {
            return configuration.GetValue<string>(SFX_DOWNLOAD_PATH);
        }

        public static string GetMusicApiAddress(this IConfiguration configuration)
        {
            return configuration.GetValue<string>(MUSIC_API_ADDRESS);
        }

        public static string GetSfxApiAddress(this IConfiguration configuration)
        {
            return configuration.GetValue<string>(SFX_API_ADDRESS);
        }

        public static string GetMusicCdnBaseAddress(this IConfiguration configuration)
        {
            return configuration.GetValue<string>(MUSIC_CDN_BASE_ADDRESS);
        }

        public static string GetSfxCdnBaseAddress(this IConfiguration configuration)
        {
            return configuration.GetValue<string>(SFX_CDN_BASE_ADDRESS);
        }

        public static string GetArtCdnBaseAddress(this IConfiguration configuration)
        {
            return configuration.GetValue<string>(ART_CDN_BASE_ADDRESS);
        }
    }
}