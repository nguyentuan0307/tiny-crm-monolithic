using System.Linq.Expressions;
using TinyCRM.Domain.Entities.Accounts;
using TinyCRM.Domain.Helper.Specification.Base;

namespace TinyCRM.Application.Helper.Specification.Accounts;

public class AccountsSpecification : Specification<Account>, ISpecification<Account>
{
    private readonly string? _keyWord;

    public AccountsSpecification(string? keyWord)
    {
        _keyWord = keyWord;
    }

    public override Expression<Func<Account, bool>> ToExpression()
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