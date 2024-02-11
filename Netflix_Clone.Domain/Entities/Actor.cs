namespace Netflix_Clone.Domain.Entities
{
    public class Actor: Person
    {
        public IEnumerable<Content>? Contents { get; set; } = new List<Content>();
        public IEnumerable<ContentActor>? ContentsActors { get; set; } = new List<ContentActor>();
    }
}
