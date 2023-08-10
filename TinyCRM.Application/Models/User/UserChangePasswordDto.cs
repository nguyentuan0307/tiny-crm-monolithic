using System.ComponentModel.DataAnnotations;

namespace TinyCRM.Application.Models.User;

public class UserChangePasswordDto
{
    [EmailAddress] public string Email { get; set; } = null!;

    [Required] public string CurrentPassword { get; set; } = null!;

    [Required] public string NewPassword { get; set; } = null!;
}