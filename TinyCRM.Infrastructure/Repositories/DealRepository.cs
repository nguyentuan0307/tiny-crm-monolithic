using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TinyCRM.Application.Helper.Specification.Deals;
using TinyCRM.Domain.Entities.Deals;
using TinyCRM.Domain.Helper.Model;
using TinyCRM.Domain.Helper.QueryParameters;

namespace TinyCRM.Infrastructure.Repositories;

public class DealRepository : Repository<Deal, Guid>, IDealRepository
{
    public DealRepository(DbFactory dbFactory) : base(dbFactory)
    {
    }

    public bool IsExistingLead(Guid leadId)
    {
        return DbSet.Any(d => d.LeadId == leadId);
    }

    public async Task<List<Deal>> GetDealsAsync(DealQueryParameters dealQueryParameters)
    {
        var specification = new DealsByFilterSpecification(dealQueryParameters.KeyWord);
        return await ListAsync(specification,
            dealQueryParameters.IncludeTables,
            dealQueryParameters.Sorting,
            dealQueryParameters.PageIndex,
            dealQueryParameters.PageSize);
    }

    public async Task<List<Deal>> GetDealsByAccountIdAsync(DealQueryParameters dealQueryParameters)
    {
        var dealsByAccountIdSpecification = new DealsByAccountIdSpecification(dealQueryParameters.AccountId!.Value);

        var specification =
            dealsByAccountIdSpecification.And(new DealsByFilterSpecification(dealQueryParameters.KeyWord));

        return await ListAsync(specification,
            dealQueryParameters.IncludeTables,
            dealQueryParameters.Sorting,
            dealQueryParameters.PageIndex,
            dealQueryParameters.PageSize);
    }

    public async Task<List<DealStatisticDto>> GetDealStatisticsAsync()
    {
        return await DbSet.Select(x => new DealStatisticDto
        {
            StatusDeal = x.StatusDeal,
            ActualRevenue = x.ActualRevenue
        }).ToListAsync();
    }

    public override Task<bool> AnyAsync(Guid id)
    {
        return DbSet.AnyAsync(p => p.Id == id);
    }

    protected override Expression<Func<Deal, bool>> ExpressionForGet(Guid id)
    {
        return p => p.Id == id;
    }
}