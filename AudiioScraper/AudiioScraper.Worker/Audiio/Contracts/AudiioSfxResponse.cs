using System.Collections.Generic;

namespace AudiioScraper.Worker.Audiio.Contracts
{
    public class AudiioSfxResponse
    {
        public int Count { get; set; }
        public List<AudiioSfxTrack> Tracks { get; set; }
    }
}
