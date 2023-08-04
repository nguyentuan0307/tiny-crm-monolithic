using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using TinyCRM.Domain.Base;
using TinyCRM.Domain.Helper.Specification.Base;
using TinyCRM.Domain.Interfaces;

namespace TinyCRM.Infrastructure
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        private readonly DbFactory _dbFactory;
        private DbSet<TEntity>? _dbSet;

        protected DbSet<TEntity> DbSet => _dbSet ??= _dbFactory.DbContext.Set<TEntity>();

        protected Repository(DbFactory dbFactory)
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

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
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

        public Task<TEntity?> GetAsync(TKey id, string? includeTables = null)
        {
            IQueryable<TEntity> query = DbSet;

            query = query.Where(ExpressionForGet(id));

            if (string.IsNullOrEmpty(includeTables)) return query.FirstOrDefaultAsync();
            var includeProperties = includeTables.Split(',');

            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return query.FirstOrDefaultAsync();
        }

        public async Task<List<TEntity>> ListAsync(ISpecification<TEntity> specification, string? includeTables = null,
            string? sorting = null, int pageIndex = 1, int pageSize = int.MaxValue)
        {
            IQueryable<TEntity> query = DbSet;

            query = Including(query, includeTables);

            query = Filter(query, specification);

            query = Sorting(query, sorting);

            query = query.Skip(pageSize * (pageIndex - 1)).Take(pageSize);
            return await query.ToListAsync();
        }

        private static IQueryable<TEntity> Filter(IQueryable<TEntity> query, ISpecification<TEntity> specification)
        {
            query = query.Where(specification.ToExpression());
            return query;
        }

        private static IQueryable<TEntity> Including(IQueryable<TEntity> query, string? includeTables = null)
        {
            if (string.IsNullOrEmpty(includeTables)) return query;
            var includeProperties = includeTables.Split(',');

            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return query;
        }

        private static IQueryable<TEntity> Sorting(IQueryable<TEntity> query, string? sorting)
        {
            return string.IsNullOrWhiteSpace(sorting) ? query : query.OrderBy(sorting);
        }

        public virtual Task<bool> AnyAsync(TKey id)
        {
            return DbSet.AnyAsync();
        }

        public Task<bool> AnyAsync()
        {
            return DbSet.AnyAsync();
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>> expression)
        {
            return DbSet.CountAsync(expression);
        }

        protected virtual Expression<Func<TEntity, bool>> ExpressionForGet(TKey id)
        {
            return p => true;
        }
    }
}