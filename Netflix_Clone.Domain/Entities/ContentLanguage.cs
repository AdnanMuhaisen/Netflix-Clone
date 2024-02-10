namespace Netflix_Clone.Domain.Entities
{
    public class ContentLanguage(int Id,string Language)
    {
        public int Id { get; init; } = Id;
        public string Language { get; init; } = Language;

        // relationships
        public IEnumerable<Content> LanguageContents { get; set; } = new List<Content>();
    }
}
