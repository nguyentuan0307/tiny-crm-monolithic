using TinyCRM.Domain.Interfaces;

namespace TinyCRM.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbFactory _dbFactory;

        public UnitOfWork(DbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public Task<int> SaveChangeAsync()
        {
            return _dbFactory.DbContext.SaveChangesAsync();
        }
    }
}