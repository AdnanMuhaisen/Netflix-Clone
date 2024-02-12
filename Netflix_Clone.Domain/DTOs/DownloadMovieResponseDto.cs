namespace Netflix_Clone.Domain.DTOs
{
    public record DownloadMovieResponseDto
    {
        public bool IsDownloaded { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
