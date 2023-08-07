using TinyCRM.Domain.Helper.Model;
using TinyCRM.Domain.Helper.QueryParameters;
using TinyCRM.Domain.Interfaces;

namespace TinyCRM.Domain.Entities.Leads;

public interface ILeadRepository : IRepository<Lead, Guid>
{
    Task<List<Lead>> GetLeadsAsync(LeadQueryParameters leadQueryParameters);

    Task<List<Lead>> GetLeadsByAccountIdAsync(LeadQueryParameters dealQueryParameters);

    Task<List<LeadStatisticDto>> GetLeadStatisticsAsync();
}