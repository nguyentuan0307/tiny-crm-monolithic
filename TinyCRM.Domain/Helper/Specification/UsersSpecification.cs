using System.Linq.Expressions;
using TinyCRM.Domain.Entities.Users;

namespace TinyCRM.Domain.Helper.Specification;

public class UsersByFilterSpecification : ISpecification<ApplicationUser>
{
    private readonly string? _keyWord;

    public UsersByFilterSpecification(string? keyWord)
    {
        _keyWord = keyWord;
    }

    public Expression<Func<ApplicationUser, bool>> IsSatisfiedBy()
    {
        Expression<Func<ApplicationUser, bool>> expression = p => true;
        if (_keyWord == null) return expression;
        if (!string.IsNullOrEmpty(_keyWord))
        {
            expression = p => p.Name.Contains(_keyWord)
                              || p.Email!.Contains(_keyWord);
        }

        return expression;
    }
}