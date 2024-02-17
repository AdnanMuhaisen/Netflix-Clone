namespace Netflix_Clone.Shared.DTOs
{
    public record AddToUserWatchHistoryRequestDto
    {
        public string ApplicationUserId { get; init; } = string.Empty;
        public int ContentId { get; init; }
    }
}
