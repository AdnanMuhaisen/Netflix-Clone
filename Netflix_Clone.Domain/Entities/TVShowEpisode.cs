namespace Netflix_Clone.Domain.Entities
{
    public class TVShowEpisode(int Id,int LengthInMinutes,int SeasonNumber)
    {
        public int Id { get; init; } = Id;
        public int LengthInMinutes { get; init; } = LengthInMinutes;
        public int SeasonNumber { get; init; } = SeasonNumber;

        //relationships
        public int TVShowId { get; set; }
        public TVShow TVShow { get; set; } = default!;
    }
}
