using InventoryEntities.Models;
using InventoryRepositories.DataAccess;
using InventoryRepositories.Repositories.Interfaces;

namespace InventoryRepositories.Repositories.Implementation
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly InventoryDbContext _dbContext;
        public ProductRepository(InventoryDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /*public void update(Product obj)
        {
            var objFromDb = _dbContext.Products.FirstOrDefault(x => x.ProductId == obj.ProductId);
            if (objFromDb != null)
            {
                objFromDb.ProductName = obj.ProductName;
                objFromDb.Description = obj.Description;
                objFromDb.Price = obj.Price;
                objFromDb.TotalQuantityOfProduct = obj.TotalQuantityOfProduct;
                objFromDb.QuantityInstock = obj.QuantityInstock;
                objFromDb.Status = obj.Status;
                objFromDb.CategoryId = obj.CategoryId;
                if (obj.ProductImageURL != null)
                {
                    objFromDb.ProductImageURL = obj.ProductImageURL;
                }
            }
        }*/
    }
}
