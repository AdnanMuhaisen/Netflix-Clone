namespace Netflix_Clone.Shared.DTOs
{
    public record ELSUpdateDocumentResponse
    {
        public bool IsUpdated { get; set; }
        public List<string>? ErrorList { get; set; }
        public string? Message { get; set; }
    }
}
