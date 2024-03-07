namespace Netflix_Clone.Shared.DTOs
{
    public record ELSDeleteDocumentResponse
    {
        public bool IsDeleted { get; set; }
        public List<string>? ErrorList { get; set; }
        public string? Message { get; set; }
    }
}
