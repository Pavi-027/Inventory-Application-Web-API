using InventoryEntities.Models;

namespace InventoryRepositories.Repositories.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void update(Category obj);
    }
}
