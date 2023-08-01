using System.Linq.Expressions;
using TinyCRM.Domain.Entities.Leads;

namespace TinyCRM.Domain.Helper.Specification;

public class LeadsSpecification : ISpecification<Lead>
{
    private readonly string? _keyWord;

    public LeadsSpecification(string? keyWord)
    {
        _keyWord = keyWord;
    }

    public Expression<Func<Lead, bool>> IsSatisfiedBy()
    {
        Expression<Func<Lead, bool>> expression = p => true;
        if (_keyWord == null) return expression;
        if (!string.IsNullOrEmpty(_keyWord))
        {
            expression = p => p.Title.Contains(_keyWord)
                              || p.Account.Name.Contains(_keyWord);
        }

        return expression;
    }
}

public class LeadsByAccountIdSpecification : ISpecification<Lead>
{
    private readonly string? _keyWord;
    private readonly Guid _accountId;

    public LeadsByAccountIdSpecification(string? keyWord, Guid accountId)
    {
        _accountId = accountId;
        _keyWord = keyWord;
    }

    public Expression<Func<Lead, bool>> IsSatisfiedBy()
    {
        Expression<Func<Lead, bool>> expression = p => p.AccountId == _accountId;

        if (string.IsNullOrEmpty(_keyWord)) return expression;
        {
            expression = p => p.AccountId == _accountId &&
                              (p.Title.Contains(_keyWord) || p.Account.Name.Contains(_keyWord));
            return expression;
        }
    }
}