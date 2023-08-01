using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TinyCRM.Domain.Entities.Leads;
using TinyCRM.Domain.Helper.Model;
using TinyCRM.Domain.Helper.QueryParameters;
using TinyCRM.Domain.Helper.Specification;

namespace TinyCRM.Infrastructure.Repositories;

public class LeadRepository : Repository<Lead, Guid>, ILeadRepository
{
    public LeadRepository(DbFactory dbFactory) : base(dbFactory)
    {
    }

    protected override Expression<Func<Lead, bool>> ExpressionForGet(Guid id)
    {
        return p => p.Id == id;
    }

    public override Task<bool> AnyAsync(Guid id)
    {
        return DbSet.AnyAsync(p => p.Id == id);
    }

    public IQueryable<Lead> GetLeads(LeadQueryParameters leadQueryParameters)
    {
        var specification = new LeadsSpecification(leadQueryParameters.KeyWord);
        return List(specification: specification,
            includeTables: leadQueryParameters.IncludeTables,
            sorting: leadQueryParameters.Sorting,
            pageIndex: leadQueryParameters.PageIndex,
            pageSize: leadQueryParameters.PageSize);
    }

    public IQueryable<Lead> GetLeadsByAccountId(LeadQueryParameters leadQueryParameters)
    {
        var specification = new LeadsByAccountIdSpecification(leadQueryParameters.KeyWord, leadQueryParameters.AccountId!.Value);
        return List(specification: specification,
            includeTables: leadQueryParameters.IncludeTables,
            sorting: leadQueryParameters.Sorting,
            pageIndex: leadQueryParameters.PageIndex,
            pageSize: leadQueryParameters.PageSize);
    }

    public IQueryable<LeadStatistic> GetLeadStatistics()
    {
        return DbSet.Select(x => new LeadStatistic
        {
            StatusLead = x.StatusLead,
            EstimatedRevenue = x.EstimatedRevenue
        });
    }
}