using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TinyCRM.Application.Helper.Specification.Leads;
using TinyCRM.Domain.Entities.Leads;
using TinyCRM.Domain.Helper.Model;
using TinyCRM.Domain.Helper.QueryParameters;

namespace TinyCRM.Infrastructure.Repositories;

public class LeadRepository : Repository<Lead, Guid>, ILeadRepository
{
    public LeadRepository(DbFactory dbFactory) : base(dbFactory)
    {
    }

    public override Task<bool> AnyAsync(Guid id)
    {
        return DbSet.AnyAsync(p => p.Id == id);
    }

    public async Task<List<Lead>> GetLeadsAsync(LeadQueryParameters leadQueryParameters)
    {
        var specification = new LeadsByFilterSpecification(leadQueryParameters.KeyWord);
        return await ListAsync(specification,
            leadQueryParameters.IncludeTables,
            leadQueryParameters.Sorting,
            leadQueryParameters.PageIndex,
            leadQueryParameters.PageSize);
    }

    public async Task<List<Lead>> GetLeadsByAccountIdAsync(LeadQueryParameters leadQueryParameters)
    {
        var leadsByAccountIdSpecification = new LeadsByAccountIdSpecification(leadQueryParameters.AccountId!.Value);
        var specification =
            leadsByAccountIdSpecification.And(new LeadsByFilterSpecification(leadQueryParameters.KeyWord));
        return await ListAsync(specification,
            leadQueryParameters.IncludeTables,
            leadQueryParameters.Sorting,
            leadQueryParameters.PageIndex,
            leadQueryParameters.PageSize);
    }

    public async Task<List<LeadStatisticDto>> GetLeadStatisticsAsync()
    {
        return await DbSet.Select(x => new LeadStatisticDto
        {
            StatusLead = x.StatusLead,
            EstimatedRevenue = x.EstimatedRevenue
        }).ToListAsync();
    }

    protected override Expression<Func<Lead, bool>> ExpressionForGet(Guid id)
    {
        return p => p.Id == id;
    }
}