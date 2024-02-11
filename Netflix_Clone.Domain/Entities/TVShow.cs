namespace Netflix_Clone.Domain.Entities
{
    public class TVShow : Content
        
    {
        public int TotalNumberOfEpisodes { get; set; } 
        public int TotalNumberOfSeasons { get; set; } 

        //relationships
        public IEnumerable<TVShowEpisode> Episodes { get; set; } = new List<TVShowEpisode>();
    }
}
