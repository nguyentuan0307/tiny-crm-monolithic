using TinyCRM.Domain.Helper.Model;
using TinyCRM.Domain.Helper.QueryParameters;
using TinyCRM.Domain.Interfaces;

namespace TinyCRM.Domain.Entities.Deals;

public interface IDealRepository : IRepository<Deal, Guid>
{
    bool IsExistingLead(Guid leadId);

    Task<List<Deal>> GetDealsAsync(DealQueryParameters dealQueryParameters);

    Task<List<Deal>> GetDealsByAccountIdAsync(DealQueryParameters dealQueryParameters);

    Task<List<DealStatisticDto>> GetDealStatisticsAsync();
}