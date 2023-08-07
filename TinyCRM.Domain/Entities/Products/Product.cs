using TinyCRM.Domain.Entities.Base;
using TinyCRM.Domain.Entities.ProductDeals;
using TinyCRM.Domain.Enums;

namespace TinyCRM.Domain.Entities.Products;

public class Product : AuditEntity<Guid>
{
    public Product()
    {
        ProductDeals = new HashSet<ProductDeal>();
    }

    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public TypeProduct TypeProduct { get; set; }
    public decimal Price { get; set; }
    public bool Status { get; set; }
    public ICollection<ProductDeal> ProductDeals { get; set; }
}