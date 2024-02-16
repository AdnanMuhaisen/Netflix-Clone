namespace Netflix_Clone.Domain.Entities
{
    public class WatchListContent
    {
        public int Id { get; set; }
        public int WatchListId { get; set; }
        public int ContentId { get; set; }

        public UserWatchList WatchList { get; set; } = default!;
        public Content Content { get; set; } = default!;
    }
}
