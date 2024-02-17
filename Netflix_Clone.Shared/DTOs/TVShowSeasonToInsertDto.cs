namespace Netflix_Clone.Shared.DTOs
{
    public record TVShowSeasonToInsertDto
    {
        public string? SeasonName { get; set; }
        public int SeasonNumber { get; set; }
        public int TVShowId { get; set; }
    }
}
