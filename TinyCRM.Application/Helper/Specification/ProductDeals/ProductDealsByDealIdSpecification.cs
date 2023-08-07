using System.Linq.Expressions;
using TinyCRM.Domain.Entities.ProductDeals;
using TinyCRM.Domain.Helper.Specification.Base;

namespace TinyCRM.Application.Helper.Specification.ProductDeals;

public class ProductDealsByDealIdSpecification : Specification<ProductDeal>, ISpecification<ProductDeal>
{
    private readonly Guid _dealId;

    public ProductDealsByDealIdSpecification(Guid dealId)
    {
        _dealId = dealId;
    }

    public override Expression<Func<ProductDeal, bool>> ToExpression()
    {
        return p => p.DealId == _dealId;
    }
}