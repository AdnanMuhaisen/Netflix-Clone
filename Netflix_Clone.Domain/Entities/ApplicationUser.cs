using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace Netflix_Clone.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required,MaxLength(30)]
        public string FirstName { get; init; } = string.Empty;
        [Required, MaxLength(30)]
        public string LastName { get; init; } = string.Empty;
    }
}
