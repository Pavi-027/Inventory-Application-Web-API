using System.Linq.Expressions;

namespace InventoryRepositories.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(string? includeProperties = null);
        Task<T> GetById(Expression<Func<T, bool>> filter, string? includeProperties = null);
        Task<IEnumerable<T>> GetByIds(Expression<Func<T, bool>> filter, string? includeProperties = null);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(T entity);
        //Task<T> DeleteRange(IEnumerable<T> entities);
    }
}