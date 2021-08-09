using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AudiioScraper.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AudiioScraper.Worker.Helpers
{
    public class DownloaderHelper
    {
        private readonly IConfiguration _configuration;
        private readonly AudiioScraperDbContext _dbContext;
        private readonly ILogger<DownloaderHelper> _logger;

        public DownloaderHelper(
            IConfiguration configuration,
            AudiioScraperDbContext dbContext,
            ILogger<DownloaderHelper> logger)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Go(CancellationToken stoppingToken)
        {
            var nextAssetToDownload =
                await _dbContext.AssetsToDownload.FirstOrDefaultAsync(x => !x.DownloadedOn.HasValue, stoppingToken);

            if (nextAssetToDownload != null)
            {
                using var webClient = new WebClient();

                var musicPath = Path.Combine(_configuration.GetValue<string>("MUSIC_DOWNLOAD_PATH"), nextAssetToDownload.AudiioFileName);
                var artistPath = Path.Combine(_configuration.GetValue<string>("MUSIC_DOWNLOAD_PATH"), nextAssetToDownload.AudiioFileName.Replace(Path.GetExtension(nextAssetToDownload.AudiioFileName), $"-artist{Path.GetExtension(nextAssetToDownload.ArtistImageFileName)}"));
                var albumPath = Path.Combine(_configuration.GetValue<string>("MUSIC_DOWNLOAD_PATH"), nextAssetToDownload.AudiioFileName.Replace(Path.GetExtension(nextAssetToDownload.AudiioFileName), $"-album{Path.GetExtension(nextAssetToDownload.AlbumImageFileName)}"));
                var musicUri = $"{_configuration.GetValue<string>("MUSIC_CDN_BASE_ADDRESS")}/{nextAssetToDownload.AudiioFileName}";
                var artistUri = $"{_configuration.GetValue<string>("ART_CDN_BASE_ADDRESS")}/{nextAssetToDownload.ArtistImageFileName}";
                var albumUri = $"{_configuration.GetValue<string>("ART_CDN_BASE_ADDRESS")}/{nextAssetToDownload.AlbumImageFileName}";

                await webClient.DownloadFileTaskAsync(musicUri, musicPath);
                await webClient.DownloadFileTaskAsync(artistUri, artistPath);
                await webClient.DownloadFileTaskAsync(albumUri, albumPath);

                nextAssetToDownload.DownloadedOn = DateTime.Now;
                await _dbContext.SaveChangesAsync(stoppingToken);
            }
        }
    }
}