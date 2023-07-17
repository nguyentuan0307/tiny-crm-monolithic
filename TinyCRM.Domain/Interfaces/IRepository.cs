using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TinyCRM.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task AddRangeAsync(List<T> enetities);
        void Remove(T entity);
        void Update(T entity);
        Task<T?> GetAsync(Expression<Func<T, bool>> expression);
        IQueryable<T> List(Expression<Func<T, bool>> expression);
    }
}
