namespace Netflix_Clone.Domain.Entities
{
    public class Award(int Id,string AwardTitle)
    {
        public int Id { get; init; } = Id;
        public string AwardTitle { get; init; } = AwardTitle;

        //relationships:
        public IEnumerable<Content> AwardContents { get; set; } = new List<Content>();
        public IEnumerable<ContentAward> ContentsAwards { get; set; } = new List<ContentAward>();



    }
}
