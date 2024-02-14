using System.Text.Json.Serialization;

namespace Netflix_Clone.Domain.Entities
{
    public class TVShow : Content
        
    {
        public int TotalNumberOfSeasons { get; set; }

        //denormalization
        public int TotalNumberOfEpisodes { get; set; }

        //relationships
        [JsonIgnore]
        public IEnumerable<TVShowSeason> Seasons { get; set; } = new List<TVShowSeason>();
    }
}
