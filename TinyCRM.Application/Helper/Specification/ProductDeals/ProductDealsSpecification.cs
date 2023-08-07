using System.Linq.Expressions;
using TinyCRM.Domain.Entities.ProductDeals;
using TinyCRM.Domain.Helper.Specification.Base;

namespace TinyCRM.Application.Helper.Specification.ProductDeals;

public class ProductDealsByFilterSpecification : Specification<ProductDeal>, ISpecification<ProductDeal>
{
    private readonly string? _keyWord;

    public ProductDealsByFilterSpecification(string? keyWord)
    {
        _keyWord = keyWord;
    }

    public override Expression<Func<ProductDeal, bool>> ToExpression()
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