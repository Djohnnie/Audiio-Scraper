using System.Collections.Generic;

namespace AudiioScraper.Worker.Audiio.Contracts
{
    public class AudiioMusicResponse
    {
        public int Count { get; set; }
        public List<AudiioMusicTrack> Tracks { get; set; }
    }
}