using System;

namespace Domain
{
    public class EpisodeNotFoundException : Exception
    {
        public string ShowName { get; set; }
        public int Season { get; set; }
        public int Episode { get; set; }
    }
}