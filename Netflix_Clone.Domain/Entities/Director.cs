namespace Netflix_Clone.Domain.Entities
{
    public class Director : Person
    {

        public IEnumerable<Content> Contents { get; set; } = new List<Content>();
    }
}
