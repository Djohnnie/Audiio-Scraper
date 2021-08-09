using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AudiioScraper.DataAccess;
using AudiioScraper.Entities;
using AudiioScraper.Entities.Enums;
using AudiioScraper.Worker.Audiio.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace AudiioScraper.Worker.Helpers
{
    public class ScraperHelper
    {
        private readonly IConfiguration _configuration;
        private readonly AudiioScraperDbContext _dbContext;
        private readonly ILogger<ScraperHelper> _logger;

        public ScraperHelper(
            IConfiguration configuration,
            AudiioScraperDbContext dbContext,
            ILogger<ScraperHelper> logger)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Go(CancellationToken stoppingToken)
        {
            string baseUrl = "https://audiio.com/api/tag/{0}";
            int page = 0;
            bool tracksFound = false;
            int numberOfNewTracks = 0;

            do
            {
                page++;
                var scrapedOn = DateTime.Now;
                var client = new RestClient(string.Format(baseUrl, page));
                var request = new RestRequest(Method.GET);
                var response = await client.ExecuteAsync<AudiioMusicResponse>(request, stoppingToken);

                if (response.Data != null)
                {
                    _logger.LogInformation("Scraper found {tracks} music tracks on page {page}", response.Data.Tracks.Count, page);

                    tracksFound = response.Data.Tracks.Any();

                    foreach (var track in response.Data.Tracks)
                    {
                        if (!await _dbContext.AssetsToDownload.AnyAsync(x => x.AudiioId == track.Id, stoppingToken))
                        {
                            await _dbContext.AssetsToDownload.AddAsync(new AssetToDownload
                            {
                                Kind = AudiioKind.Music,
                                AudiioId = track.Id,
                                Title = track.Title,
                                Artist = track.Artist?.Name,
                                Album = track.Album?.Title,
                                AudiioFileName = track.Sound_Pro.Replace("/", ""),
                                ArtistImageFileName = track.Artist?.Image?.Replace("/", ""),
                                AlbumImageFileName = track.Album?.Image?.Replace("/", ""),
                                ScrapedOn = scrapedOn,
                            }, stoppingToken);

                            numberOfNewTracks++;
                        }
                    }

                    await _dbContext.SaveChangesAsync(stoppingToken);
                }

            } while (tracksFound);

            _logger.LogInformation($"{numberOfNewTracks} new music tracks scraped!");
        }
    }
}