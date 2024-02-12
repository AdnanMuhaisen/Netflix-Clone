using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Netflix_Clone.Domain.DTOs
{
    public record RegistrationRequestDto
    {
        [Required,MinLength(1),MaxLength(200)]
        public string FirstName { get; set; } = string.Empty;

        [Required, MinLength(1), MaxLength(200)]
        public string LastName { get; set; } = string.Empty;

        [Required,EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required, Phone]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
