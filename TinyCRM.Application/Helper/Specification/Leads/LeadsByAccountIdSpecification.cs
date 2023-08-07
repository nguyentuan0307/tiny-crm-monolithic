using System.Linq.Expressions;
using TinyCRM.Domain.Entities.Leads;
using TinyCRM.Domain.Helper.Specification.Base;

namespace TinyCRM.Application.Helper.Specification.Leads;

public class LeadsByAccountIdSpecification : Specification<Lead>, ISpecification<Lead>
{
    private readonly Guid _accountId;

    public LeadsByAccountIdSpecification(Guid accountId)
    {
        _accountId = accountId;
    }

    public override Expression<Func<Lead, bool>> ToExpression()
    {
        return p => p.AccountId == _accountId;
    }
}