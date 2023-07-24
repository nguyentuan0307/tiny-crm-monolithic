using System.ComponentModel.DataAnnotations;

namespace TinyCRM.API.Models.ProductDeal
{
    public class ProductDealCreateDto
    {
        [Required(ErrorMessage = "DealId is Required")]
        public Guid DealId { get; set; }

        [Required(ErrorMessage = "ProductId is Required")]
        public Guid ProductId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be non-negative")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be non-negative")]
        public int Quantity { get; set; }
    }
}