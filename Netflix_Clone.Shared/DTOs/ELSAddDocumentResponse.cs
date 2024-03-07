namespace Netflix_Clone.Shared.DTOs
{
    public record ELSAddDocumentResponse
    {
        public bool IsAdded { get; set; }
        public List<string>? ErrorList { get; set; }
        public string? Message { get; set; }
    }
}
