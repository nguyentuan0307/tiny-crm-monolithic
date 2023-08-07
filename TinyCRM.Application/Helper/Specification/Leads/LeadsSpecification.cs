using System.Linq.Expressions;
using TinyCRM.Domain.Entities.Leads;
using TinyCRM.Domain.Helper.Specification.Base;

namespace TinyCRM.Application.Helper.Specification.Leads;

public class LeadsByFilterSpecification : Specification<Lead>, ISpecification<Lead>
{
    private readonly string? _keyWord;

    public LeadsByFilterSpecification(string? keyWord)
    {
        _keyWord = keyWord;
    }

    public override Expression<Func<Lead, bool>> ToExpression()
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