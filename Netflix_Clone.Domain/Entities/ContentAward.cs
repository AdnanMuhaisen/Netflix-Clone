namespace Netflix_Clone.Domain.Entities
{
    public class ContentAward
    {
        public int Id { get; set; }
        public int ContentId { get; set; }
        public int AwardId { get; set; }

        public Content Content { get; set; } = default!;
        public Award Award { get; set; } = default!;
    }
}
