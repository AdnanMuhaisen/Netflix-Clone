namespace Netflix_Clone.Domain.Entities
{
    public class Tag(int Id,string TagValue)
    {
        public int Id { get; init; } = Id;
        public string TagValue { get; init; } = TagValue;


        //relationships
        public IEnumerable<Content> TagContents { get; set; } = new List<Content>();
        public IEnumerable<ContentTag> ContentsTags { get; set; } = new List<ContentTag>();

    }
}
