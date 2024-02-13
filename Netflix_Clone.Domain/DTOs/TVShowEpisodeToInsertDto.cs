using System.ComponentModel.DataAnnotations;

namespace Netflix_Clone.Domain.DTOs
{
    public record TVShowEpisodeToInsertDto
    {
        [Required] public int LengthInMinutes { get; init; }
        [Required] public int SeasonNumber { get; init; }
        [Required] public int EpisodeNumber { get; init; }
        [Required] public int TVShowId { get; set; }
        [Required] public string Location { get; set; } = string.Empty;
        [Required] public int SeasonId { get; set; }
    }
}
