using InventoryEntities.Models;
using InventoryRepositories.DataAccess;
using InventoryRepositories.Repositories.Interfaces;

namespace InventoryRepositories.Repositories.Implementation
{
    public class SalesOrderRepository : Repository<SalesOrder>, ISalesOrderRepository
    {
        private readonly InventoryDbContext _dbContext;
        public SalesOrderRepository(InventoryDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
