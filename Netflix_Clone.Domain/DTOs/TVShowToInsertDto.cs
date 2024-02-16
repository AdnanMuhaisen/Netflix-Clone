using System.ComponentModel.DataAnnotations;

namespace Netflix_Clone.Domain.DTOs
{
    public record TVShowToInsertDto
    {
        [Required, Range(1960, 2024)] public int ReleaseYear { get; set; }
        [Required] public string Title { get; set; } = string.Empty;
        [Required, Range(1, 150)] public int MinimumAgeToWatch { get; set; }
        [Required, MaxLength(500)] public string Synopsis { get; set; } = string.Empty;
        [Required, Range(1, 2)] public int LanguageId { get; set; }
        [Required] public int ContentGenreId { get; set; }
        [Required] public int DirectorId { get; set; }
        [Required] public bool IsAvailableToDownload { get; set; }
        public List<ContentTagDto> Tags { get; set; } = new List<ContentTagDto>();
    }
}
