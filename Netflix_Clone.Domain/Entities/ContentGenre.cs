namespace Netflix_Clone.Domain.Entities
{
    public class ContentGenre(int Id,string Genre)
    {
        public int Id { get; init; } = Id;
        public string Genre { get; init; } = Genre;

        //relationships

        public IEnumerable<Content> GenreContents { get; set; } = new List<Content>();
    }
}
