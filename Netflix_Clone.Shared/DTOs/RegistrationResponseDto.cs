using System.ComponentModel.DataAnnotations;

namespace Netflix_Clone.Shared.DTOs
{
    public record RegistrationResponseDto
    {
        public string UserId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public required bool IsRegistered { get; set; }
    }
}
