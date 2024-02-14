using System.Text.Json.Serialization;

namespace Netflix_Clone.Domain.Entities
{
    public class TVShowEpisode
    {
        public int Id { get; set; } 
        public int LengthInMinutes { get; init; } 
        public int SeasonNumber { get; init; }
        public int EpisodeNumber { get; init; }
        public string FileName { get; set; } = string.Empty;

        //relationships
        public int TVShowId { get; set; }

        public int SeasonId { get; set; }
        [JsonIgnore]
        public TVShowSeason Season { get; set; } = default!;
    }
}
