using System.ComponentModel.DataAnnotations;

namespace Netflix_Clone.Shared.DTOs
{
    public record DownloadEpisodeRequestDto
    {
        public required int TVShowId { get; set; }
        public required int SeasonId { get; set; }
        public required int EpisodeId { get; set; }

        [MaxLength(100)]
        public string PathToDownloadFor { get; set; } = string.Empty;
    }
}
