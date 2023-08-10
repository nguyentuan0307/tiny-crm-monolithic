using Microsoft.EntityFrameworkCore.Storage;
using TinyCRM.Domain.Interfaces;

namespace TinyCRM.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbFactory _dbFactory;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(DbFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public void BeginTransaction()
    {
        if (_transaction == null)
            _transaction = _dbFactory.DbContext.Database.BeginTransaction();
        else
            throw new InvalidOperationException("Transaction already started.");
    }

    public void Rollback()
    {
        if (_transaction != null)
        {
            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
        }
        else
        {
            throw new InvalidOperationException("Transaction not started.");
        }
    }

    public void Commit()
    {
        if (_transaction != null)
        {
            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
        }
        else
        {
            throw new InvalidOperationException("Transaction not started.");
        }
    }

    public async Task<int> SaveChangeAsync()
    {
        if (_transaction != null)
            return await _dbFactory.DbContext.SaveChangesAsync();
        throw new InvalidOperationException("Transaction not started. Call BeginTransaction() before saving changes.");
    }
}