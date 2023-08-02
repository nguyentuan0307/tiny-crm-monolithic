using System.Linq.Expressions;
using TinyCRM.Domain.Entities.Deals;

namespace TinyCRM.Domain.Helper.Specification;

public class DealsByFilterSpecification : ISpecification<Deal>
{
    private readonly string? _keyWord;

    public DealsByFilterSpecification(string? keyWord)
    {
        _keyWord = keyWord;
    }

    public Expression<Func<Deal, bool>> IsSatisfiedBy()
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

public class DealsByAccountIdSpecification : ISpecification<Deal>
{
    private readonly Guid _accountId;

    public DealsByAccountIdSpecification(Guid accountId)
    {
        _accountId = accountId;
    }

    public Expression<Func<Deal, bool>> IsSatisfiedBy()
    {
        return p => p.Lead.AccountId == _accountId;
    }
}