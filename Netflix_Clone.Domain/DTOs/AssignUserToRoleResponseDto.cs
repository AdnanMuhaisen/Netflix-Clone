namespace Netflix_Clone.Domain.DTOs
{
    public record AssignUserToRoleResponseDto
    {
        public required bool IsAssigned { get; set; }
    }
}
