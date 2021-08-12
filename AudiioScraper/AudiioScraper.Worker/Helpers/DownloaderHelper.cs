using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AudiioScraper.Common.Extensions;
using AudiioScraper.DataAccess;
using AudiioScraper.Entities.Enums;
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
                try
                {
                    using var webClient = new WebClient();

                    string downloadMusicPath = _configuration.GetMusicDownloadPath();
                    string downloadSfxPath = _configuration.GetSfxDownloadPath();
                    string musicCdnBaseAddress = _configuration.GetMusicCdnBaseAddress();
                    string sfxCdnBaseAddress = _configuration.GetSfxCdnBaseAddress();
                    string artCdnBaseAddress = _configuration.GetArtCdnBaseAddress();

                    switch (nextAssetToDownload.Kind)
                    {
                        case AudiioKind.Music:
                            var musicPath = Path.Combine(downloadMusicPath, nextAssetToDownload.AudiioFileName);
                            var musicUri = $"{musicCdnBaseAddress}/{nextAssetToDownload.AudiioFileName}";
                            var musicArtistPath = Path.Combine(downloadMusicPath,
                                nextAssetToDownload.AudiioFileName.Replace(
                                    Path.GetExtension(nextAssetToDownload.AudiioFileName),
                                    $"-artist{Path.GetExtension(nextAssetToDownload.ArtistImageFileName)}"));
                            var musicAlbumPath = Path.Combine(downloadMusicPath,
                                nextAssetToDownload.AudiioFileName.Replace(
                                    Path.GetExtension(nextAssetToDownload.AudiioFileName),
                                    $"-album{Path.GetExtension(nextAssetToDownload.AlbumImageFileName)}"));
                            var musicArtistUri = $"{artCdnBaseAddress}/{nextAssetToDownload.ArtistImageFileName}";
                            var musicAlbumUri = $"{artCdnBaseAddress}/{nextAssetToDownload.AlbumImageFileName}";

                            await webClient.DownloadFileTaskAsync(musicUri, musicPath);
                            await webClient.DownloadFileTaskAsync(musicArtistUri, musicArtistPath);
                            await webClient.DownloadFileTaskAsync(musicAlbumUri, musicAlbumPath);

                            _logger.LogInformation(
                                $"Dowloaded music track '{nextAssetToDownload.Title}' successfully.");

                            break;
                        case AudiioKind.Sfx:
                            var sfxPath = Path.Combine(downloadSfxPath, nextAssetToDownload.AudiioFileName);
                            var sfxUri = $"{sfxCdnBaseAddress}/{nextAssetToDownload.AudiioFileName}";
                            var sfxArtistPath = Path.Combine(downloadSfxPath,
                                nextAssetToDownload.AudiioFileName.Replace(
                                    Path.GetExtension(nextAssetToDownload.AudiioFileName),
                                    $"-artist{Path.GetExtension(nextAssetToDownload.ArtistImageFileName)}"));
                            var sfxAlbumPath = Path.Combine(downloadSfxPath,
                                nextAssetToDownload.AudiioFileName.Replace(
                                    Path.GetExtension(nextAssetToDownload.AudiioFileName),
                                    $"-album{Path.GetExtension(nextAssetToDownload.AlbumImageFileName)}"));
                            var sfxArtistUri = $"{artCdnBaseAddress}/{nextAssetToDownload.ArtistImageFileName}";
                            var sfxAlbumUri = $"{artCdnBaseAddress}/{nextAssetToDownload.AlbumImageFileName}";

                            await webClient.DownloadFileTaskAsync(sfxUri, sfxPath);
                            await webClient.DownloadFileTaskAsync(sfxArtistUri, sfxArtistPath);
                            await webClient.DownloadFileTaskAsync(sfxAlbumUri, sfxAlbumPath);

                            _logger.LogInformation($"Dowloaded SFX track '{nextAssetToDownload.Title}' successfully.");

                            break;
                    }

                    nextAssetToDownload.DownloadedOn = DateTime.Now;
                    await _dbContext.SaveChangesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Dowload failed! ({ex.Message})");
                }
            }
        }
    }
}