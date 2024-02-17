namespace Netflix_Clone.Shared.DTOs
{ 
    public record UserWatchListDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public List<ContentTagDto> WatchListContents { get; set; } = new List<ContentTagDto>();
    }
}
