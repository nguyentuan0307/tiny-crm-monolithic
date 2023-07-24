using TinyCRM.Domain.Interfaces;

namespace TinyCRM.Domain.Entities.Deals
{
    public interface IDealRepository : IRepository<Deal>
    {
        public bool IsExistingLead(Guid leadId);
    }
}