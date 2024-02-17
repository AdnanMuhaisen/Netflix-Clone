using System.ComponentModel.DataAnnotations;

namespace Netflix_Clone.Shared.DTOs
{
    public record DownloadMovieRequestDto
    {
        public required int MovieId { get; set; }
        [MaxLength(100)]
        public string PathToDownloadFor { get; set; } = string.Empty;
    }
}
