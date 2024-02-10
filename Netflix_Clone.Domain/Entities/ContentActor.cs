namespace Netflix_Clone.Domain.Entities
{
    public class ContentActor
    {
        public int Id { get; set; }
        public int ContentId { get; set; }
        public int ActorId { get; set; }
        public Actor Actor { get; set; } = default!;
        public Content Content { get; set; } = default!;
    }
}
