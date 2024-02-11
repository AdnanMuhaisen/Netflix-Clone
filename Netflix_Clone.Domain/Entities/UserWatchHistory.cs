namespace Netflix_Clone.Domain.Entities
{
    public class UserWatchHistory
    {
        public int Id { get; init; }
        public string ApplicationUserId { get; init; } = string.Empty;
        public int ContentId { get; init; }

        public ApplicationUser ApplicationUser { get; set; } = default!;
        public Content Content { get; set; } = default!;
    }
}
