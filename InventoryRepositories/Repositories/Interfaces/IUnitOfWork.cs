namespace InventoryRepositories.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        ICustomerRepository Customer { get; }
        ISupplierRepository Supplier { get; }
        ISalesOrderRepository SalesOrder { get; }
        IPurchaseOrderRepository PurchaseOrder { get; }
        void save();
        void DetachAllEntities();
    }
}
