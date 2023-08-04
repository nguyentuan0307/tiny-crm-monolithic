using System.Linq.Expressions;
using TinyCRM.Domain.Helper.Specification.Base;

namespace TinyCRM.Domain.Helper.Specification.Contacts;

public class ContactsByFilterSpecification : Specification<Entities.Contacts.Contact>, ISpecification<Entities.Contacts.Contact>
{
    private readonly string? _keyWord;

    public ContactsByFilterSpecification(string? keyWord)
    {
        _keyWord = keyWord;
    }

    public override Expression<Func<Entities.Contacts.Contact, bool>> ToExpression()
    {
        Expression<Func<Entities.Contacts.Contact, bool>> expression = p => true;
        if (_keyWord == null) return expression;
        if (!string.IsNullOrEmpty(_keyWord))
        {
            expression = p => p.Name.Contains(_keyWord)
                              || p.Email.Contains(_keyWord);
        }

        return expression;
    }
}