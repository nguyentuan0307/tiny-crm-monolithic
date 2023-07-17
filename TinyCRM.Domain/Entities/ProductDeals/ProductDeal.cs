using TinyCRM.Domain.Base;
using TinyCRM.Domain.Entities.Deals;
using TinyCRM.Domain.Entities.Products;

namespace TinyCRM.Domain.Entities.ProductDeals
{
    public class ProductDeal : AuditEntity<Guid>
    {
        public Guid DealId { get; set; }
        public Deal? Deal { get; set; }
        public string ProductId { get; set; } = null!;
        public Product? Product { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
