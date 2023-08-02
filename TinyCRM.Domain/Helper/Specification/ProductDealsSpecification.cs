using System.Linq.Expressions;
using TinyCRM.Domain.Entities.ProductDeals;

namespace TinyCRM.Domain.Helper.Specification;

public class ProductDealsByFilterSpecification : ISpecification<ProductDeal>
{
    private readonly string? _keyWord;

    public ProductDealsByFilterSpecification(string? keyWord)
    {
        _keyWord = keyWord;
    }

    public Expression<Func<ProductDeal, bool>> IsSatisfiedBy()
    {
        Expression<Func<ProductDeal, bool>> expression = p => true;
        if (_keyWord == null) return expression;
        if (!string.IsNullOrEmpty(_keyWord))
        {
            expression = p => p.Product.Code.Contains(_keyWord) || p.Product.Name.Contains(_keyWord);
        }

        return expression;
    }
}

public class ProductDealsByDealIdSpecification : ISpecification<ProductDeal>
{
    private readonly Guid _dealId;

    public ProductDealsByDealIdSpecification(Guid dealId)
    {
        _dealId = dealId;
    }

    public Expression<Func<ProductDeal, bool>> IsSatisfiedBy()
    {
        return p => p.DealId == _dealId;
    }
}