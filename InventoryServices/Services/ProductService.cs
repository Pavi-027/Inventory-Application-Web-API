using InventoryDTO.DTO.Product_Dto;
using InventoryEntities.Models;
using InventoryRepositories.Repositories.Interfaces;
using InventoryServices.Services.ServiceInterface;

namespace InventoryServices.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ProductDTO>> GetAll()
        {
            var product = await _unitOfWork.Product.GetAll(includeProperties: "Category");

            var productDTO = new List<ProductDTO>();
            foreach (var item in product)
            {
                productDTO.Add(new ProductDTO()
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Description = item.Description,
                    Price = item.Price,
                    Discount = item.Discount,
                    TotalQuantityOfProduct = item.TotalQuantityOfProduct,
                    QuantityInstock = item.QuantityInstock,
                    Status = item.Status,
                    Category = item.Category
                });
            }
            return productDTO;
        }

        public async Task<IEnumerable<Product>> GetByIds(int[] ids)
        {
            return (await _unitOfWork.Product.GetByIds(x => ids.Contains(x.ProductId), includeProperties: "Category")).ToList();
        }

        public async Task<ProductRequestDto> GetById(int id)
        {
            var product = await _unitOfWork.Product.GetById(x => x.ProductId == id, includeProperties: "Category");

            if (product == null)
            {
                return null;
            }
            var productDTO = new ProductRequestDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Description = product.Description,
                Price = product.Price,
                Discount = product.Discount,
                TotalQuantityOfProduct = product.TotalQuantityOfProduct,
                QuantityInstock = product.QuantityInstock,
                Status = product.Status,
                Category = product.Category
            };
            return productDTO;
        }
        public async Task<AddProductRequestDTO> Create(AddProductRequestDTO addProductDTO)
        {
            var product = new Product()
            {
                ProductName = addProductDTO.ProductName,
                Description = addProductDTO.Description,
                Price = addProductDTO.Price,
                Discount = addProductDTO.Discount,
                TotalQuantityOfProduct = addProductDTO.TotalQuantityOfProduct,
                QuantityInstock = addProductDTO.QuantityInstock,
                Status = addProductDTO.Status,
                CategoryId = addProductDTO.CategoryId,
            };

            _unitOfWork.Product.Add(product);
            _unitOfWork.save();

            var productDTO = new AddProductRequestDTO
            {
                ProductName = product.ProductName,
                Description = product.Description,
                Price = product.Price,
                Discount = product.Discount,
                TotalQuantityOfProduct = product.TotalQuantityOfProduct,
                QuantityInstock = product.QuantityInstock,
                Status = product.Status,
                CategoryId = product.CategoryId
            };
            return productDTO;
        }
        public async Task<UpdateProductRequestDTO> Update(int id, UpdateProductRequestDTO updateProductRequestDTO)
        {
            var product = _unitOfWork.Product.GetById(x => x.ProductId == id, includeProperties: "Category");
            if (product == null)
            {
                return null;
            }
            _unitOfWork.DetachAllEntities();

            //Map DTO to Model
            var productModel = new Product
            {
                ProductId = id,
                ProductName = updateProductRequestDTO.ProductName,
                Description = updateProductRequestDTO.Description,
                Price = updateProductRequestDTO.Price,
                Discount = updateProductRequestDTO.Discount,
                TotalQuantityOfProduct = updateProductRequestDTO.TotalQuantityOfProduct,
                QuantityInstock = updateProductRequestDTO.QuantityInstock,
                Status = updateProductRequestDTO.Status,
                CategoryId = updateProductRequestDTO.CategoryId

            };

            //check if region exists
            productModel = await _unitOfWork.Product.Update(productModel);
            _unitOfWork.save();

            if (productModel == null)
            {
                return null;
            }

            //Convert Model to DTO
            var productDTO = new UpdateProductRequestDTO
            {
                ProductName = productModel.ProductName,
                Description = productModel.Description,
                Price = productModel.Price,
                Discount = productModel.Discount,
                TotalQuantityOfProduct = productModel.TotalQuantityOfProduct,
                QuantityInstock = productModel.QuantityInstock,
                Status = productModel.Status,
                CategoryId = productModel.CategoryId
            };
            return productDTO;
        }
        public async Task<Product> DeleteById(int id)
        {
            Product deleteProductById = await _unitOfWork.Product.GetById(x => x.ProductId == id, includeProperties: "Category");

            if (deleteProductById == null)
            {
                return null;
            }
            await _unitOfWork.Product.Delete(deleteProductById);
            _unitOfWork.save();
            return deleteProductById;
        }
    }
}