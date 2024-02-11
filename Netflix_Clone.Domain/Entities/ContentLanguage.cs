namespace Netflix_Clone.Domain.Entities
{
    public class ContentLanguage
    {
        public int Id { get; init; }
        public string Language { get; init; } = string.Empty;

        // relationships
        public IEnumerable<Content> LanguageContents { get; set; } = new List<Content>();
    }
}
