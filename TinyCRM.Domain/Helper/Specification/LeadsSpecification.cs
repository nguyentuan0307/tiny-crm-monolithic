using System.Linq.Expressions;
using TinyCRM.Domain.Entities.Leads;

namespace TinyCRM.Domain.Helper.Specification;

public class LeadsByFilterSpecification : ISpecification<Lead>
{
    private readonly string? _keyWord;

    public LeadsByFilterSpecification(string? keyWord)
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
    private readonly Guid _accountId;

    public LeadsByAccountIdSpecification(Guid accountId)
    {
        _accountId = accountId;
    }

    public Expression<Func<Lead, bool>> IsSatisfiedBy()
    {
        return p => p.AccountId == _accountId;
    }
}