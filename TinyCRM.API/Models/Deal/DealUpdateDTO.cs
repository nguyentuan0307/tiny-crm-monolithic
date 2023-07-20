using System.ComponentModel.DataAnnotations;

namespace TinyCRM.API.Models.Deal
{
    public class DealUpdateDTO
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }
    }
}