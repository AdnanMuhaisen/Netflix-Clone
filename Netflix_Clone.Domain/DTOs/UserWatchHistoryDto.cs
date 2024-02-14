namespace Netflix_Clone.Domain.DTOs
{
    public record UserWatchHistoryDto
    {
        public int Id { get; init; }
        public string ApplicationUserId { get; init; } = string.Empty;
        public int ContentId { get; init; }
    }
}
