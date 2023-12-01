using InventoryRepositories.DataAccess;
using InventoryRepositories.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InventoryRepositories.Repositories.Implementation
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly InventoryDbContext _dbContext;
        internal DbSet<T> dbSet;
        public Repository(InventoryDbContext dbContext)
        {
            _dbContext = dbContext;
            dbSet = _dbContext.Set<T>();
        }

        public async Task<T> Add(T entity)
        {
            dbSet.Add(entity);
            return entity;
        }

        //Category, Sales
        public async Task<IEnumerable<T>> GetAll(string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }

        public async Task<IEnumerable<T>> GetByIds(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }
        public async Task<T> GetById(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();
        }

        public async Task<T> Update(T entity)
        {
            dbSet.Update(entity);
            return entity;
        }

        public async Task<T> Delete(T entity)
        {
            dbSet.Remove(entity);
            return entity;
        }

        /*public async Task<T> DeleteRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
            return entities;
        }*/
    }
}