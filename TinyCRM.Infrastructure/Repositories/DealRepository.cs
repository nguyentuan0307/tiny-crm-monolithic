using TinyCRM.Domain.Entities.Deals;

namespace TinyCRM.Infrastructure.Repositories
{
    public class DealRepository : Repository<Deal>, IDealRepository
    {
        public DealRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public bool IsExistingDeal(Guid leadId)
        {
            return DbSet.Any(d => d.LeadId == leadId);
        }
    }
}