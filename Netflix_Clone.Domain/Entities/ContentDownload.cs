namespace Netflix_Clone.Domain.Entities
{
    public class ContentDownload
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; } = string.Empty;
        public int ContentId { get; set; }


        public ApplicationUser ApplicationUser { get; set; } = null!;
        public Content Content { get; set; } = null!;
    }
}
