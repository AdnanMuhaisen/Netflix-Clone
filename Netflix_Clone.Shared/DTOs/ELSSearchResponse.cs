namespace Netflix_Clone.Shared.DTOs
{
    public record ELSSearchResponse<T> where T : class
    {
        public IEnumerable<T> Result { get; set; } = Enumerable.Empty<T>();
        public List<string>? ErrorList { get; set; }
        public string? Message { get; set; }
    }
}
