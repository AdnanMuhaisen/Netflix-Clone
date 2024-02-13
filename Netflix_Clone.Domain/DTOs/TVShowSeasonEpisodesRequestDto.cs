namespace Netflix_Clone.Domain.DTOs
{
    public record TVShowSeasonEpisodesRequestDto
    {
        public required int TVShowId { get; set; }
        public required int TVShowSeasonId { get; set; }
    }
}
