namespace Netflix_Clone.Domain.DTOs
{
    public record TVShowSeasonEpisodeToDeleteDto
    {
        public required int TVShowID { get; set; }
        public required int TVShowSeasonID { get; set; }
        public required int EpisodeID { get; set; }
    }
}
