using System;
using AudiioScraper.Entities.Enums;

namespace AudiioScraper.Entities
{
    public class AssetToDownload
    {
        public Guid Id { get; set; }
        public int SysId { get; set; }
        public AudiioKind Kind { get; set; }
        public int AudiioId { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string AudiioFileName { get; set; }
        public string ArtistImageFileName { get; set; }
        public string AlbumImageFileName { get; set; }
        public DateTime ScrapedOn { get; set; }
        public DateTime? DownloadedOn { get; set; }
    }
}