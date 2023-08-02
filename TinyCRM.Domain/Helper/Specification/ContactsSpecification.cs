using System.Linq.Expressions;

namespace TinyCRM.Domain.Helper.Specification;

public class ContactsByFilterSpecification : ISpecification<Entities.Contacts.Contact>
{
    private readonly string? _keyWord;

    public ContactsByFilterSpecification(string? keyWord)
    {
        _keyWord = keyWord;
    }

    public Expression<Func<Entities.Contacts.Contact, bool>> IsSatisfiedBy()
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

public class ContactsByAccountIdSpecification : ISpecification<Entities.Contacts.Contact>
{
    private readonly Guid _accountId;

    public ContactsByAccountIdSpecification(Guid accountId)
    {
        _accountId = accountId;
    }

    public Expression<Func<Entities.Contacts.Contact, bool>> IsSatisfiedBy()
    {
        return p => p.AccountId == _accountId;
    }
}