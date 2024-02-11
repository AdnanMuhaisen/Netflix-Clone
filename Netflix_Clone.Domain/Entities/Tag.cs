namespace Netflix_Clone.Domain.Entities
{
    public class Tag
    {
        public int Id { get; init; }
        public string TagValue { get; init; } = string.Empty;


        //relationships
        public IEnumerable<Content> TagContents { get; set; } = new List<Content>();
        public IEnumerable<ContentTag> ContentsTags { get; set; } = new List<ContentTag>();

    }
}
