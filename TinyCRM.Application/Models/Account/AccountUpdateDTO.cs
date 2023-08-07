using System.ComponentModel.DataAnnotations;

namespace TinyCRM.Application.Models.Account;

public class AccountUpdateDto
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = null!;

    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Phone Number Required!")]
    [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
        ErrorMessage = "Entered phone format is not valid.")]
    public string Phone { get; set; } = null!;

    public string? Address { get; set; }
}