using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TinyCRM.Domain.Base;
using TinyCRM.Domain.Entities.Deals;

namespace TinyCRM.Infrastructure.Repositories
{
    public class DealRepository : Repository<Deal>, IDealRepository
    {
        public DealRepository(DbFactory dbFactory) : base(dbFactory) { }

        public bool IsExistingDeal(Guid leadId)
        {
            return DbSet.Any(d => d.LeadId == leadId);
        }

        public override Task<Deal?> GetAsync(Expression<Func<Deal, bool>> expression)
        {
            return DbSet.Include(p => p.Lead).ThenInclude(a => a.Account).Include(p => p.ProductDeals)
                .ThenInclude(u => u.Product)
                .FirstOrDefaultAsync(expression);
        }

        public override void Update(Deal entity)
        {
            if (entity is IAuditEntity auditEntity)
            {
                auditEntity.UpdatedDate = DateTime.UtcNow;
            }
            DbSet.Update(entity);
        }
    }
}
