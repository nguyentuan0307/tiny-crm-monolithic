using TinyCRM.Domain.Entities.Leads;

namespace TinyCRM.Infrastructure.Repositories
{
    public class LeadRepository : Repository<Lead>, ILeadRepository
    {
        public LeadRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}