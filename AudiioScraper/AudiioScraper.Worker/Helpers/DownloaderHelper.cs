using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AudiioScraper.Common.Extensions;
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

                string downloadMusicPath = _configuration.GetMusicDownloadPath();
                string musicCdnBaseAddress = _configuration.GetMusicCdnBaseAddress();
                string artCdnBaseAddress = _configuration.GetArtCdnBaseAddress();

                var musicPath = Path.Combine(downloadMusicPath, nextAssetToDownload.AudiioFileName);
                var artistPath = Path.Combine(downloadMusicPath, nextAssetToDownload.AudiioFileName.Replace(Path.GetExtension(nextAssetToDownload.AudiioFileName), $"-artist{Path.GetExtension(nextAssetToDownload.ArtistImageFileName)}"));
                var albumPath = Path.Combine(downloadMusicPath, nextAssetToDownload.AudiioFileName.Replace(Path.GetExtension(nextAssetToDownload.AudiioFileName), $"-album{Path.GetExtension(nextAssetToDownload.AlbumImageFileName)}"));
                var musicUri = $"{musicCdnBaseAddress}/{nextAssetToDownload.AudiioFileName}";
                var artistUri = $"{artCdnBaseAddress}/{nextAssetToDownload.ArtistImageFileName}";
                var albumUri = $"{artCdnBaseAddress}/{nextAssetToDownload.AlbumImageFileName}";

                await webClient.DownloadFileTaskAsync(musicUri, musicPath);
                await webClient.DownloadFileTaskAsync(artistUri, artistPath);
                await webClient.DownloadFileTaskAsync(albumUri, albumPath);

                nextAssetToDownload.DownloadedOn = DateTime.Now;
                await _dbContext.SaveChangesAsync(stoppingToken);
            }
        }
    }
}