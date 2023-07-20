using System.Text.Json.Serialization;
using TinyCRM.Domain.Enums;

namespace TinyCRM.API.Models.Product
{
    public class ProductDTO
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