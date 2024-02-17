namespace Netflix_Clone.Shared.DTOs
{
    public record UserWatchHistoryDto
    {
        public int Id { get; init; }
        public string ApplicationUserId { get; init; } = string.Empty;
        public int ContentId { get; init; }
    }
}
