namespace Netflix_Clone.Domain.Entities
{
    public class TVShow(int Id, string Title, int ReleaseYear, int MinimumAgeToWatch, string Synopsis, string Location,int TotalNumberOfEpisodes, int TotalNumberOfSeasons) 
        : Content(Id, Title,  ReleaseYear, MinimumAgeToWatch, Synopsis, Location)
    {
        public int TotalNumberOfEpisodes { get; set; } = TotalNumberOfEpisodes;
        public int TotalNumberOfSeasons { get; set; } = TotalNumberOfSeasons;

        //relationships
        public IEnumerable<TVShowEpisode> Episodes { get; set; } = new List<TVShowEpisode>();
    }
}
