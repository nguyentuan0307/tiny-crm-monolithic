using System.Linq.Expressions;
using TinyCRM.Domain.Helper.Specification;

namespace TinyCRM.Domain.Interfaces
{
    public interface IRepository<TEntity, in TKey> where TEntity : class
    {
        Task AddAsync(TEntity entity);

        Task AddRangeAsync(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);

        void Update(TEntity entity);

        Task<TEntity?> GetAsync(TKey id, string? stringInclude = null);

        IQueryable<TEntity> List(ISpecification<TEntity> specification, string? includeTables = null,
            string? sorting = null, int pageIndex = 1, int pageSize = int.MaxValue);

        Task<bool> AnyAsync(TKey id);

        Task<bool> AnyAsync();

        Task<int> CountAsync(Expression<Func<TEntity, bool>> expression);
    }
}