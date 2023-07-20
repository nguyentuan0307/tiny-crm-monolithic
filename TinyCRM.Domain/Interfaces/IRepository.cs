using System.Linq.Expressions;

namespace TinyCRM.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);

        Task AddRangeAsync(List<T> enetities);

        void Remove(T entity);

        void Update(T entity);

        Task<T?> GetAsync(Expression<Func<T, bool>> expression, string? stringInclude = default);

        IQueryable<T> List(Expression<Func<T, bool>>? expression = null);

        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);

        Task<int> CountAsync(Expression<Func<T, bool>> expression);
    }
}