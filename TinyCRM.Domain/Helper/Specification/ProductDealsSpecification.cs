using System.Linq.Expressions;
using TinyCRM.Domain.Entities.ProductDeals;

namespace TinyCRM.Domain.Helper.Specification;

public class ProductDealsByDealIdSpecification : ISpecification<ProductDeal>
{
    private readonly string? _keyWord;
    private readonly Guid _dealId;

    public ProductDealsByDealIdSpecification(string? keyWord, Guid dealId)
    {
        _keyWord = keyWord;
        _dealId = dealId;
    }

    public Expression<Func<ProductDeal, bool>> IsSatisfiedBy()
    {
        Expression<Func<ProductDeal, bool>> expression = p => p.DealId == _dealId;
        if (_keyWord == null) return expression;
        if (!string.IsNullOrEmpty(_keyWord))
        {
            expression = p => p.DealId == _dealId &&
                (p.Product.Code.Contains(_keyWord) || p.Product.Name.Contains(_keyWord));
        }

        return expression;
    }
}