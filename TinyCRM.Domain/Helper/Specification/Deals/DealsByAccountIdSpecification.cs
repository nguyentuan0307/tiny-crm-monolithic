using System.Linq.Expressions;
using TinyCRM.Domain.Entities.Deals;
using TinyCRM.Domain.Helper.Specification.Base;

namespace TinyCRM.Domain.Helper.Specification.Deals;

public class DealsByAccountIdSpecification : Specification<Deal>, ISpecification<Deal>
{
    private readonly Guid _accountId;

    public DealsByAccountIdSpecification(Guid accountId)
    {
        _accountId = accountId;
    }

    public override Expression<Func<Deal, bool>> ToExpression()
    {
        return p => p.Lead.AccountId == _accountId;
    }
}