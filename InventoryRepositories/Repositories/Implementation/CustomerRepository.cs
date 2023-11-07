using InventoryEntities.Models;
using InventoryRepositories.DataAccess;
using InventoryRepositories.Repositories.Interfaces;

namespace InventoryRepositories.Repositories.Implementation
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private readonly InventoryDbContext _dbContext;
        public CustomerRepository(InventoryDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
