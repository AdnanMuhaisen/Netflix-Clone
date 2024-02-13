namespace Netflix_Clone.Domain.DTOs
{
    public record AssignUserToRoleRequestDto
    {
        public required string UserId { get; set; }
        public required string RoleName { get; set; }
    }
}
