namespace Netflix_Clone.Domain.Entities
{
    public class ContentTag
    {
        public int Id { get; set; }
        public int ContentId { get; set; }
        public int TagId { get; set; }

        public Content Content { get; set; } = default!;
        public Tag Tag { get; set; } = default!;

    }
}
