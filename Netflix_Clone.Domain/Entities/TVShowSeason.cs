using System.Text.Json.Serialization;

namespace Netflix_Clone.Domain.Entities
{
    public class TVShowSeason
    {
        public int Id { get; set; }
        public string? SeasonName { get; set; }
        public string DirectoryName { get; set; } = string.Empty;
        public int SeasonNumber { get; set; }
        public int TotalNumberOfEpisodes { get; set; }

        //relationships
        public int TVShowId { get; set; }
        [JsonIgnore]
        public TVShow TVShow { get; set; } = default!;

        [JsonIgnore]
        public ICollection<TVShowEpisode> Episodes { get; set; } = new List<TVShowEpisode>();
    }
}
