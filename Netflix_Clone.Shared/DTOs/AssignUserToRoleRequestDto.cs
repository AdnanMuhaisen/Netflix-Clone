namespace Netflix_Clone.Shared.DTOs
{
    public record AssignUserToRoleRequestDto
    {
        public required string UserId { get; set; }
        public required string RoleName { get; set; }
    }
}
