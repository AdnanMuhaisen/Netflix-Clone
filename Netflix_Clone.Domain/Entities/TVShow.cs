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

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is not TVShow) return false;

            var targetObject = obj as TVShow;

            return base.Equals(obj)
                && this.TotalNumberOfSeasons == targetObject!.TotalNumberOfSeasons
                && this.TotalNumberOfEpisodes == targetObject!.TotalNumberOfEpisodes;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode()
                * (23 + TotalNumberOfSeasons.GetHashCode())
                * (23 + TotalNumberOfEpisodes.GetHashCode());
        }
    }
}
