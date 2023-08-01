using System.Linq.Expressions;
using TinyCRM.Domain.Entities.Deals;

namespace TinyCRM.Domain.Helper.Specification;

public class DealsSpecification : ISpecification<Deal>
{
    private readonly string? _keyWord;

    public DealsSpecification(string? keyWord)
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
    private readonly string? _keyWord;
    private readonly Guid _accountId;

    public DealsByAccountIdSpecification(string? keyWord, Guid accountId)
    {
        _accountId = accountId;
        _keyWord = keyWord;
    }

    public Expression<Func<Deal, bool>> IsSatisfiedBy()
    {
        Expression<Func<Deal, bool>> expression = p => p.Lead.AccountId == _accountId;

        if (string.IsNullOrEmpty(_keyWord)) return expression;
        {
            expression = p => p.Lead.AccountId == _accountId &&
                              (p.Title.Contains(_keyWord) || p.Lead.Account.Name.Contains(_keyWord));
            return expression;
        }
    }
}