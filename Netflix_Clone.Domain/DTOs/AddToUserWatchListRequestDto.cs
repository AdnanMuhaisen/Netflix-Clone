namespace Netflix_Clone.Domain.DTOs
{
    public record AddToUserWatchListRequestDto
    {
        public required string UserId { get; set; } = string.Empty;
        public required int ContentId { get; set; }
    }
}
