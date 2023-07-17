﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TinyCRM.Domain.Base;
using TinyCRM.Domain.Interfaces;

namespace TinyCRM.Infrastructure
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbFactory _dbFactory;
        private DbSet<TEntity> _dbSet;

        protected DbSet<TEntity> DbSet
        {
            get => _dbSet ??= _dbFactory.DbContext.Set<TEntity>();
        }

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

        public async Task AddRangeAsync(List<TEntity> enetities)
        {
            await DbSet.AddRangeAsync(enetities);
        }

        public void Remove(TEntity entity)
        {

            DbSet.Remove(entity);
        }

        public void Update(TEntity entity)
        {
            if (entity is IAuditEntity auditEntity)
            {
                auditEntity.UpdatedDate = DateTime.UtcNow;
            }
            DbSet.Update(entity);
        }

        public Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression)
        {
            return DbSet.FirstOrDefaultAsync(expression);
        }

        public IQueryable<TEntity> List(Expression<Func<TEntity, bool>> expression)
        {
            return DbSet.Where(expression);
        }
    }
}
