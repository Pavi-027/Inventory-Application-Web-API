using InventoryEntities.Models;
using InventoryRepositories.DataAccess;
using InventoryRepositories.Repositories.Interfaces;

namespace InventoryRepositories.Repositories.Implementation
{
    public class SupplierRepository : Repository<Supplier>, ISupplierRepository
    {
        private readonly InventoryDbContext _dbContext;
        public SupplierRepository(InventoryDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
