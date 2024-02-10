namespace Netflix_Clone.Domain.Entities
{
    public class Actor(int Id, string FirstName, string LastName, string? Email = null, string? ImageUrl = null)
        : Person(Id, FirstName, LastName, Email, ImageUrl)
    {
        public IEnumerable<Content>? Contents { get; set; } = new List<Content>();
        public IEnumerable<ContentActor>? ContentsActors { get; set; } = new List<ContentActor>();
    }
}
