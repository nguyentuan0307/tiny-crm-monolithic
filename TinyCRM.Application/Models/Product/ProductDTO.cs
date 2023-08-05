using System.Text.Json.Serialization;
using TinyCRM.Domain.Enums;

namespace TinyCRM.Application.Models.Product
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TypeProduct TypeProduct { get; set; }

        public decimal Price { get; set; }
        public bool Status { get; set; }
    }
}