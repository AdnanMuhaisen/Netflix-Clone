namespace Netflix_Clone.Shared.DTOs
{
    public record LoginResponseDto
    {
        public required ApplicationUserDto UserDto { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
