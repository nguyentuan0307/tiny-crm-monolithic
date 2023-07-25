using Microsoft.EntityFrameworkCore.Storage;
using TinyCRM.Domain.Interfaces;

namespace TinyCRM.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbFactory _dbFactory;
        private IDbContextTransaction _transaction = null!;

        public UnitOfWork(DbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public void BeginTransaction()
        {
            _transaction = _dbFactory.DbContext.Database.BeginTransaction();
        }

        public void Rollback()
        {
            _transaction?.Rollback();
        }

        public void Commit()
        {
            _transaction?.Commit();
        }

        public Task<int> SaveChangeAsync()
        {
            return _dbFactory.DbContext.SaveChangesAsync();
        }
    }
}