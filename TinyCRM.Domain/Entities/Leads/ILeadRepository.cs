using TinyCRM.Domain.Helper.Model;
using TinyCRM.Domain.Helper.QueryParameters;
using TinyCRM.Domain.Interfaces;

namespace TinyCRM.Domain.Entities.Leads
{
    public interface ILeadRepository : IRepository<Lead, Guid>
    {
        IQueryable<Lead> GetLeads(LeadQueryParameters leadQueryParameters);

        IQueryable<Lead> GetLeadsByAccountId(LeadQueryParameters dealQueryParameters);

        IQueryable<LeadStatistic> GetLeadStatistics();
    }
}