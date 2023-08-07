using System.Linq.Expressions;
using TinyCRM.Domain.Entities.Deals;
using TinyCRM.Domain.Helper.Specification.Base;

namespace TinyCRM.Application.Helper.Specification.Deals;

public class DealsByFilterSpecification : Specification<Deal>, ISpecification<Deal>
{
    private readonly string? _keyWord;

    public DealsByFilterSpecification(string? keyWord)
    {
        _keyWord = keyWord;
    }

    public override Expression<Func<Deal, bool>> ToExpression()
    {
        Expression<Func<Deal, bool>> expression = p => true;
        if (_keyWord == null) return expression;
        if (!string.IsNullOrEmpty(_keyWord))
        {
            expression = p => p.Title.Contains(_keyWord)
                              || p.Lead.Account.Name.Contains(_keyWord);
        }

        return expression;
    }
}