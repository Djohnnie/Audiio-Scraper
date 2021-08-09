namespace AudiioScraper.Worker.Audiio.Contracts
{
    public class AudiioMusicTrack
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Sound_Pro { get; set; }
        public AudiioArtist Artist { get; set; }
        public AudiioAlbum Album { get; set; }
    }
}