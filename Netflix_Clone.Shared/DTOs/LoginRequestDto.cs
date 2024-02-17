using System.ComponentModel.DataAnnotations;

namespace Netflix_Clone.Shared.DTOs
{
    public record LoginRequestDto
    {
        [EmailAddress]
        public required string Email { get; set; }

        public required string Password { get; set; }
    }
}
