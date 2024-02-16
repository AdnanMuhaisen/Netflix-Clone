namespace Netflix_Clone.Domain.Entities
{
    public class UserWatchList
    {
        public int Id { get; set; }

        //relationships:
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser ApplicationUser { get; set; } = default!;

        public ICollection<Content> WatchListContents { get; set; } = new List<Content>();
        public ICollection<WatchListContent> WatchListsContents { get; set; } = new List<WatchListContent>();
    }
}
