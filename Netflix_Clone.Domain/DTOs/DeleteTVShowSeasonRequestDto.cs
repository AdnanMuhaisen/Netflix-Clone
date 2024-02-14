namespace Netflix_Clone.Domain.DTOs
{
    public record DeleteTVShowSeasonRequestDto
    {
        public required int TVShowId { get; set; }

        //one of the following are required to get the season
        public int TVShowSeasonNumber { get; set; }
        public int TVShowSeasonId { get; set; }
    }
}
