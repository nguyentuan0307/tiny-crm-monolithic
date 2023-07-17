using TinyCRM.Domain.Enums;

namespace TinyCRM.API.Models.Product
{
    public class ProductCreateDTO
    {
        public string Id { get; set; }
        public string Name { get; set; } = null!;
        public TypeProduct TypeProduct { get; set; }
        public decimal Price { get; set; }
    }
}
