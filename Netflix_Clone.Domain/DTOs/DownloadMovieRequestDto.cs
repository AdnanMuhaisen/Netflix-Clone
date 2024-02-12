using System.ComponentModel.DataAnnotations;

namespace Netflix_Clone.Domain.DTOs
{
    public record DownloadMovieRequestDto
    {
        public required int MovieId { get; set; }
        [MaxLength(100)]
        public required string PathToDownloadFor { get; set; } = string.Empty;
    }
}
