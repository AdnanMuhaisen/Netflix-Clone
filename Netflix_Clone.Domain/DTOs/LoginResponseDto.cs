namespace Netflix_Clone.Domain.DTOs
{
    public record LoginResponseDto
    {
        public required ApplicationUserDto UserDto { get; set; }
        public string Token { get; set; } = string.Empty;
        public required string Message { get; set; }
    }
}
