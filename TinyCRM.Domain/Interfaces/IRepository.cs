using System.Linq.Expressions;

namespace TinyCRM.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);

        Task AddRangeAsync(List<T> entities);

        void Remove(T entity);

        void Update(T entity);

        Task<T?> GetAsync(Expression<Func<T, bool>> expression, string? stringInclude = null);

        IQueryable<T> List(Expression<Func<T, bool>>? expression = null, string? includeTables = null,
            string? sorting = null, int pageIndex = 1, int pageSize = int.MaxValue);

        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);

        Task<bool> AnyAsync();

        Task<int> CountAsync(Expression<Func<T, bool>> expression);
    }
}