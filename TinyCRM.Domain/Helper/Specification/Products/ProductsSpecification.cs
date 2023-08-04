using System.Linq.Expressions;
using TinyCRM.Domain.Entities.Products;
using TinyCRM.Domain.Helper.Specification.Base;

namespace TinyCRM.Domain.Helper.Specification.Products;

public class ProductsByFilterSpecification : Specification<Product>, ISpecification<Product>
{
    private readonly string? _keyWord;

    public ProductsByFilterSpecification(string? keyWord)
    {
        _keyWord = keyWord;
    }

    public override Expression<Func<Product, bool>> ToExpression()
    {
        Expression<Func<Product, bool>> expression = p => true;
        if (_keyWord == null) return expression;
        if (!string.IsNullOrEmpty(_keyWord))
        {
            expression = p => p.Name.Contains(_keyWord)
                              || p.Code.Contains(_keyWord);
        }

        return expression;
    }
}