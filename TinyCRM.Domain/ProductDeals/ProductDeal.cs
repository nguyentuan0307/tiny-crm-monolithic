using TinyCRM.Domain.Base;
using TinyCRM.Domain.Deals;
using TinyCRM.Domain.Products;

namespace TinyCRM.Domain.ProductDeals
{
    public class ProductDeal : AuditEntity<Guid>
    {
        public Guid IdDeal { get; set; }
        public Deal? Deal { get; set; }
        public Guid IdProduct { get; set; }
        public Product? Product { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
