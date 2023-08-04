using TinyCRM.Domain.Helper.QueryParameters;
using TinyCRM.Domain.Interfaces;
using TinyCRM.Infrastructure.Helpers.Model;

namespace TinyCRM.Domain.Entities.Deals;

public interface IDealRepository : IRepository<Deal, Guid>
{
    bool IsExistingLead(Guid leadId);

    Task<List<Deal>> GetDealsAsync(DealQueryParameters dealQueryParameters);

    Task<List<Deal>> GetDealsByAccountIdAsync(DealQueryParameters dealQueryParameters);

    Task<List<DealStatistic>> GetDealStatisticsAsync();
}