namespace Netflix_Clone.Domain.DTOs
{
    public record ApiResponseDto
    {
        public required object Result { get; set; } = default!;
        public string Message { get; set; } = string.Empty;
    }
}
