using System.ComponentModel.DataAnnotations;

namespace TinyCRM.Application.Models.User;

public class SignInDto
{
    [Required] public string Email { get; set; } = null!;

    [Required] public string Password { get; set; } = null!;
}