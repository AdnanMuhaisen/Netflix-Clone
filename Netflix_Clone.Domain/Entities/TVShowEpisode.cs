namespace Netflix_Clone.Domain.Entities
{
    public class TVShowEpisode
    {
        public int Id { get; init; } 
        public int LengthInMinutes { get; init; } 
        public int SeasonNumber { get; init; } 

        //relationships
        public int TVShowId { get; set; }
        public TVShow TVShow { get; set; } = default!;
    }
}
