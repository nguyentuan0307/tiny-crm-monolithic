using System.Linq.Expressions;
using TinyCRM.Domain.Entities.Accounts;

namespace TinyCRM.Domain.Helper.Specification;

public class AccountsSpecification : ISpecification<Account>
{
    private readonly string? _keyWord;

    public AccountsSpecification(string? keyWord)
    {
        _keyWord = keyWord;
    }

    public Expression<Func<Account, bool>> IsSatisfiedBy()
    {
        Expression<Func<Account, bool>> expression = p => true;
        if (_keyWord == null) return expression;
        if (!string.IsNullOrEmpty(_keyWord))
        {
            expression = p => p.Name.Contains(_keyWord)
                              || p.Email.Contains(_keyWord);
        }

        return expression;
    }
}

public class AccountSpecification : ISpecification<Account>
{
    private readonly Guid _accountId;

    public AccountSpecification(Guid accountId)
    {
        _accountId = accountId;
    }

    public Expression<Func<Account, bool>> IsSatisfiedBy()
    {
        Expression<Func<Account, bool>> expression = p => p.Id == _accountId;
        return expression;
    }
}