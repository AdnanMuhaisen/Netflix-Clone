namespace Netflix_Clone.Domain.DTOs
{
    public record TVShowEpisodeRequestDto
    {
        public required int TVShowId { get; set; }
        public required int EpisodeId { get; set; }
        public required int TVShowSeasonId { get; set; }
    }




}
