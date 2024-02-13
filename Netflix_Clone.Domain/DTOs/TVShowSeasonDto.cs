using Netflix_Clone.Domain.Entities;

namespace Netflix_Clone.Domain.DTOs
{
    public record TVShowSeasonDto
    {
        public int Id { get; set; }
        public string? SeasonName { get; set; }
        public int SeasonNumber { get; set; }
        public int TotalNumberOfEpisodes { get; set; }
        public int TVShowId { get; set; }
        public List<TVShowEpisodeDto> Episodes { get; set; } = default!;
    }
}
