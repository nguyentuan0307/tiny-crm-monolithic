using System.ComponentModel.DataAnnotations;

namespace TinyCRM.API.Models.User
{
    public class ProfileUserUpdateDto
    {
        [Required]
        public string Name { get; set; } = null!;

        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}