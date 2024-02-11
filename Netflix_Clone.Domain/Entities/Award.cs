namespace Netflix_Clone.Domain.Entities
{
    public class Award
    {
        public int Id { get; init; } 
        public string AwardTitle { get; init; }= string.Empty;

        //relationships:
        public IEnumerable<Content> AwardContents { get; set; } = new List<Content>();
        public IEnumerable<ContentAward> ContentsAwards { get; set; } = new List<ContentAward>();



    }
}
