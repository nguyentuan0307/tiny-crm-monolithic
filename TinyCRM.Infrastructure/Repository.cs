using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using TinyCRM.Domain.Base;
using TinyCRM.Domain.Interfaces;

namespace TinyCRM.Infrastructure
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbFactory _dbFactory;
        private DbSet<TEntity>? _dbSet;

        protected DbSet<TEntity> DbSet => _dbSet ??= _dbFactory.DbContext.Set<TEntity>();

        public Repository(DbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task AddAsync(TEntity entity)
        {
            if (entity is IAuditEntity auditEntity)
            {
                auditEntity.CreatedDate = DateTime.UtcNow;
            }
            await DbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(List<TEntity> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        public void Remove(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public virtual void Update(TEntity entity)
        {
            if (entity is IAuditEntity auditEntity)
            {
                auditEntity.UpdatedDate = DateTime.UtcNow;
            }
            DbSet.Update(entity);
        }

        public virtual Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression, string? includeTables = null)
        {
            IQueryable<TEntity> query = DbSet;
            if (!string.IsNullOrEmpty(includeTables))
            {
                string[] includeProperties = includeTables.Split(',');

                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.FirstOrDefaultAsync(expression);
        }

        public IQueryable<TEntity> List(Expression<Func<TEntity, bool>>? expression = null, string? includeTables = null,
            string? sorting = null, int pageIndex = 1, int pageSize = int.MaxValue)
        {
            IQueryable<TEntity> query = DbSet;
            if (!string.IsNullOrEmpty(includeTables))
            {
                string[] includeProperties = includeTables.Split(',');

                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            if (expression != null)
            {
                query = query.Where(expression);
            }
            if (!string.IsNullOrWhiteSpace(sorting))
            {
                query = query.OrderBy(sorting);
            }
            query = query.Skip(pageSize * (pageIndex - 1)).Take(pageSize);
            return query;
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
        {
            return DbSet.AnyAsync(expression);
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>> expression)
        {
            return DbSet.CountAsync(expression);
        }
    }
}