using TinyCRM.Domain.Helper.QueryParameters;
using TinyCRM.Domain.Interfaces;
using TinyCRM.Infrastructure.Helpers.Model;

namespace TinyCRM.Domain.Entities.Deals
{
    public interface IDealRepository : IRepository<Deal, Guid>
    {
        public bool IsExistingLead(Guid leadId);

        public IQueryable<Deal> GetDeals(DealQueryParameters dealQueryParameters);

        public IQueryable<Deal> GetDealsByAccountId(DealQueryParameters dealQueryParameters);

        IQueryable<DealStatistic> GetDealStatistics();
    }
}