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




        //relationships
        public IEnumerable<Content> UserHistory { get; set; } = new List<Content>();
        public IEnumerable<UserWatchHistory> UsersHistory { get; set; } = new List<UserWatchHistory>();
    }
}
