using System.ComponentModel.DataAnnotations;

namespace Netflix_Clone.Shared.DTOs
{
    public record MovieDto {

        [Required] public int Id { get; set; }
        [Required, Range(1960, 2024)] public int ReleaseYear { get; set; }
        [Required] public string Title { get; set; } = string.Empty;
        [Required, Range(1, 150)] public int MinimumAgeToWatch { get; set; }
        [Required, MaxLength(500)] public string Synopsis { get; set; } = string.Empty;
        [Required] public string Location { get; set; } = string.Empty;
        [Required, Range(3, 180)] public int LengthInMinutes { get; set; }
        [Required, Range(1, 2)] public int LanguageId { get; set; }
        [Required] public int ContentGenreId { get; set; }
        [Required] public int DirectorId { get; set; }
        public int TotalNumberOfDownloads { get; set; } = 0;
        public bool IsAvailableToDownload { get; set; }
    }
}
