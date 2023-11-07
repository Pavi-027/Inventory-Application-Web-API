using InventoryRepositories.DataAccess;
using InventoryRepositories.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryRepositories.Repositories.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private InventoryDbContext _dbContext;
        public ICategoryRepository Category { get;}
        public IProductRepository Product { get; }
        public ICustomerRepository Customer { get; }
        public ISupplierRepository Supplier { get; }
        public ISalesOrderRepository SalesOrder { get; private set; }
        public IPurchaseOrderRepository PurchaseOrder { get; private set; }

        public UnitOfWork(InventoryDbContext dbContext, 
            ICategoryRepository _category, 
            IProductRepository _product, 
            ICustomerRepository _customer, 
            ISupplierRepository _supplier, 
            ISalesOrderRepository _salesOrderRepository, 
            IPurchaseOrderRepository _purchaseOrderRepository)
        {
            _dbContext = dbContext;
            Category = _category;
            Product = _product;
            Customer = _customer;
            Supplier = _supplier;
            SalesOrder = _salesOrderRepository;
            PurchaseOrder = _purchaseOrderRepository;
        }

        public void save()
        {
            _dbContext.SaveChanges();
        }
        public void DetachAllEntities()
        {
            _dbContext.ChangeTracker.Clear();
        }
    }
}
