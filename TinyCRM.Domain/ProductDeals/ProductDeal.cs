using TinyCRM.Domain.Base;
using TinyCRM.Domain.Deals;
using TinyCRM.Domain.Products;

namespace TinyCRM.Domain.ProductDeals
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
