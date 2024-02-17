namespace Netflix_Clone.Shared.DTOs
{
    public record ApiResponseDto
    {
        public required object Result { get; set; } = default!;
        public string Message { get; set; } = string.Empty;
    }
}
