namespace Netflix_Clone.Domain.Entities
{
    public class TVShow : Content
        
    {
        public int TotalNumberOfSeasons { get; set; }

        //denormalization
        public int TotalNumberOfEpisodes { get; set; }

        //relationships
        public IEnumerable<TVShowSeason> Seasons { get; set; } = new List<TVShowSeason>();
    }
}
