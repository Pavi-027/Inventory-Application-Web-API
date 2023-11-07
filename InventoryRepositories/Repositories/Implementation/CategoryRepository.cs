using InventoryEntities.Models;
using InventoryRepositories.DataAccess;
using InventoryRepositories.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryRepositories.Repositories.Implementation
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly InventoryDbContext _dbContext;
        public CategoryRepository(InventoryDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void update(Category obj)
        {
            _dbContext.Categories.Update(obj);
        }
    }
}
