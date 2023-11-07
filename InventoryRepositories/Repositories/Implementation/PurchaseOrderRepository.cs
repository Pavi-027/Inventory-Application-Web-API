using InventoryEntities.Models;
using InventoryRepositories.DataAccess;
using InventoryRepositories.Repositories.Interfaces;

namespace InventoryRepositories.Repositories.Implementation
{
    public class PurchaseOrderRepository : Repository<PurchaseOrder>, IPurchaseOrderRepository
    {
        private readonly InventoryDbContext _dbContext;
        public PurchaseOrderRepository(InventoryDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
