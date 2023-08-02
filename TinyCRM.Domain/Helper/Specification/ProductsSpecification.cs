using System.Linq.Expressions;
using TinyCRM.Domain.Entities.Products;

namespace TinyCRM.Domain.Helper.Specification;

public class ProductsByFilterSpecification : ISpecification<Product>
{
    private readonly string? _keyWord;

    public ProductsByFilterSpecification(string? keyWord)
    {
        _keyWord = keyWord;
    }

    public Expression<Func<Product, bool>> IsSatisfiedBy()
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