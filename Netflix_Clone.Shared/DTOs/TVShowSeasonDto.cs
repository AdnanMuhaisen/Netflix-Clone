namespace Netflix_Clone.Shared.DTOs
{
    public record TVShowSeasonDto
    {
        public int Id { get; set; }
        public string? SeasonName { get; set; }
        public string DirectoryName { get; set; } = string.Empty;
        public int SeasonNumber { get; set; }
        public int TotalNumberOfEpisodes { get; set; }
        public int TVShowId { get; set; }
        public List<TVShowEpisodeDto> Episodes { get; set; } = default!;
    }
}
