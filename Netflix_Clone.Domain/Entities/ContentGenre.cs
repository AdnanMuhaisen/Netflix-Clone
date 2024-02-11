namespace Netflix_Clone.Domain.Entities
{
    public class ContentGenre
    {
        public int Id { get; init; }
        public string Genre { get; init; } = string.Empty;

        //relationships

        public IEnumerable<Content> GenreContents { get; set; } = new List<Content>();
    }
}
