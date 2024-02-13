namespace Netflix_Clone.Domain.DTOs
{
    public record AssignUserToRoleResponseDto
    {
        public required bool IsAssigned { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
