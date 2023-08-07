using System.Linq.Expressions;
using TinyCRM.Domain.Entities.Contacts;
using TinyCRM.Domain.Helper.Specification.Base;

namespace TinyCRM.Application.Helper.Specification.Contacts;

public class ContactsByAccountIdSpecification : Specification<Contact>, ISpecification<Contact>
{
    private readonly Guid _accountId;

    public ContactsByAccountIdSpecification(Guid accountId)
    {
        _accountId = accountId;
    }

    public override Expression<Func<Contact, bool>> ToExpression()
    {
        return p => p.AccountId == _accountId;
    }
}