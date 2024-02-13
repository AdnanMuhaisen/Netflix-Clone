namespace Netflix_Clone.Domain.Entities
{
    public class TVShowSeason
    {
        public int Id { get; set; }
        public string? SeasonName { get; set; }
        public int SeasonNumber { get; set; }
        public int TotalNumberOfEpisodes { get; set; }

        //relationships
        public int TVShowId { get; set; }
        public TVShow TVShow { get; set; } = default!;

        public IEnumerable<TVShowEpisode> Episodes { get; set; } = new List<TVShowEpisode>();
    }
}
