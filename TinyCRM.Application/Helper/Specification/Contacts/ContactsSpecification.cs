using System.Linq.Expressions;
using TinyCRM.Domain.Entities.Contacts;
using TinyCRM.Domain.Helper.Specification.Base;

namespace TinyCRM.Application.Helper.Specification.Contacts;

public class ContactsByFilterSpecification : Specification<Contact>, ISpecification<Contact>
{
    private readonly string? _keyWord;

    public ContactsByFilterSpecification(string? keyWord)
    {
        _keyWord = keyWord;
    }

    public override Expression<Func<Contact, bool>> ToExpression()
    {
        Expression<Func<Contact, bool>> expression = p => true;
        if (_keyWord == null) return expression;
        if (!string.IsNullOrEmpty(_keyWord))
            expression = p => p.Name.Contains(_keyWord)
                              || p.Email.Contains(_keyWord);

        return expression;
    }
}