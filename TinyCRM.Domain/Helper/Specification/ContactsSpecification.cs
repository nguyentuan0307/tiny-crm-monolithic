using System.Linq.Expressions;

namespace TinyCRM.Domain.Helper.Specification;

public class ContactsSpecification : ISpecification<Entities.Contacts.Contact>
{
    private readonly string? _keyWord;

    public ContactsSpecification(string? keyWord)
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
    private readonly string? _keyWord;

    public ContactsByAccountIdSpecification(string? keyWord, Guid accountId)
    {
        _accountId = accountId;
        _keyWord = keyWord;
    }

    public Expression<Func<Entities.Contacts.Contact, bool>> IsSatisfiedBy()
    {
        Expression<Func<Entities.Contacts.Contact, bool>> expression = p => p.AccountId == _accountId;

        if (string.IsNullOrEmpty(_keyWord)) return expression;
        {
            expression = p => p.AccountId == _accountId &&
                              (p.Name.Contains(_keyWord) || p.Email.Contains(_keyWord));
            return expression;
        }
    }
}