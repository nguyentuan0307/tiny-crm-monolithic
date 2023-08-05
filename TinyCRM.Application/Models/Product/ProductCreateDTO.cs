using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TinyCRM.Domain.Enums;

namespace TinyCRM.Application.Models.Product
{
    public class ProductCreateDto
    {
        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; } = null!;

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = null!;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [EnumDataType(typeof(TypeProduct))]
        public TypeProduct TypeProduct { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be non-negative")]
        public decimal Price { get; set; }
    }
}