using TinyCRM.Domain.Base;
using TinyCRM.Domain.Entities.ProductDeals;
using TinyCRM.Domain.Enums;

namespace TinyCRM.Domain.Entities.Products
{
    public class Product : AuditEntity<string>
    {
        public Product()
        {
            ProductDeals = new HashSet<ProductDeal>();
        }
        public string Name { get; set; } = null!;
        public TypeProduct TypeProduct { get; set; }
        public decimal Price { get; set; }
        public bool Status { get; set; }
        public ICollection<ProductDeal> ProductDeals { get; set; }
    }
}
