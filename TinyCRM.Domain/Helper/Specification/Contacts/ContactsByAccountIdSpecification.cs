using System.Linq.Expressions;
using TinyCRM.Domain.Helper.Specification.Base;

namespace TinyCRM.Domain.Helper.Specification.Contacts;

public class ContactsByAccountIdSpecification : Specification<Entities.Contacts.Contact>, ISpecification<Entities.Contacts.Contact>
{
    private readonly Guid _accountId;

    public ContactsByAccountIdSpecification(Guid accountId)
    {
        _accountId = accountId;
    }

    public override Expression<Func<Entities.Contacts.Contact, bool>> ToExpression()
    {
        return p => p.AccountId == _accountId;
    }
}