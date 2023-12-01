using InventoryDTO.DTO.Product_Dto;
using InventoryDTO.DTO.SalesOrder_Dto;
using InventoryEntities.Models;
using InventoryRepositories.Repositories.Interfaces;
using InventoryServices.Services.ServiceInterface;
using System.Linq;

namespace InventoryServices.Services
{
    public class SalesOrderService : ISalesOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductService _productService;
        public SalesOrderService(IUnitOfWork unitOfWork, IProductService productService)
        {
            _unitOfWork = unitOfWork;
            _productService = productService;
        }

        public async Task<IEnumerable<SalesOrder>> GetAll()
        {
            var getAllSales = await _unitOfWork.SalesOrder.GetAll(includeProperties: "Customer,Products");
            return getAllSales;
            /*var salesOrder = await _unitOfWork.SalesOrder.GetAll(includeProperties: "Customer,Products");

            var salesOrderDTO = new List<SalesOrderDTO>();
            foreach (var item in salesOrder)
            {
                //var products = item.Products.Select(p => p.ProductId).ToArray();

                salesOrderDTO.Add(new SalesOrder()
                {
                    SalesId = item.SalesId,
                    SaleDate = DateTime.Now,
                    QuantityForSale = item.QuantityForSale,
                    TotalCostOfSalesOrder = item.TotalCostOfSalesOrder,
                    SalesDiscount = item.SalesDiscount,
                    CustomerId = item.CustomerId,
                    Products = products
                });
            }
            return salesOrderDTO;*/
        }
        public async Task<SalesOrderDTO> GetById(int id)
        {
            var salesOrder = await _unitOfWork.SalesOrder.GetById(x => x.SalesId == id, includeProperties: "Customer,Products");

            if (salesOrder == null)
            {
                return null;
            }
            var products = salesOrder.Products.Select(p => p.ProductId).ToArray();

            var salesOrderDTO = new SalesOrderDTO
            {
                SalesId = salesOrder.SalesId,
                SaleDate = DateTime.Now,
                QuantityForSale = salesOrder.QuantityForSale,
                TotalCostOfSalesOrder = salesOrder.TotalCostOfSalesOrder,
                SalesDiscount = salesOrder.SalesDiscount,
                CustomerId = salesOrder.CustomerId,
                Products = products
            };
            return salesOrderDTO;
        }
        public async Task<SalesOrderResponseDTO> Create(AddSalesorderRequestDTO addSalesOrderRequestDTO)
        {
            //Map or Convert DTO to Model
            var salesOrder = new SalesOrder()
            {
                SaleDate = DateTime.Now,
                QuantityForSale = addSalesOrderRequestDTO.QuantityForSale,
                TotalCostOfSalesOrder = addSalesOrderRequestDTO.TotalCostOfSalesOrder,
                SalesDiscount = addSalesOrderRequestDTO.SalesDiscount,
                CustomerId = addSalesOrderRequestDTO.CustomerId
            };

            /*IEnumerable<ProductDTO> products = await _productService.GetAll();

            int[] productId = products.Select(p => p.ProductId).ToArray();*/

            var existingProduct = await _productService.GetByIds(addSalesOrderRequestDTO.Products);

            salesOrder.Products = existingProduct;

            /*// Calculate total cost based on the prices of selected products
            salesOrder.TotalCostOfSalesOrder = CalculateTotalCost(existingProduct);*/

            //Use Model to Create Region
            salesOrder = await _unitOfWork.SalesOrder.Add(salesOrder);
            _unitOfWork.save();

            //Map Model Back to DTO
            var salesOrderDTO = new SalesOrderResponseDTO
            {
                SaleDate = salesOrder.SaleDate,
                QuantityForSale = salesOrder.QuantityForSale,
                TotalCostOfSalesOrder = salesOrder.TotalCostOfSalesOrder,
                SalesDiscount = salesOrder.SalesDiscount,
                CustomerId = salesOrder.CustomerId,
                Products = salesOrder.Products
            };
            return salesOrderDTO;
        }

        // Helper method to calculate the total cost based on product prices
        /*private decimal CalculateTotalCost(IEnumerable<Product> products)
        {
            return products.Sum(product => product.Price);
        }*/

        public async Task<SalesOrder> DeleteById(int id)
        {
            SalesOrder deleteSalesOrderById = await _unitOfWork.SalesOrder.GetById(x => x.SalesId == id, includeProperties: "Customer,Products");

            if (deleteSalesOrderById == null)
            {
                return null;
            }
            await _unitOfWork.SalesOrder.Delete(deleteSalesOrderById);
            _unitOfWork.save();
            return deleteSalesOrderById;
        }

        /*public async Task<UpdateSalesOrderRequestDTO> Update(int id, UpdateSalesOrderRequestDTO updateSalesorderRequestDTO)
        {
            var salesOrder = _unitOfWork.SalesOrder.GetById(x => x.SalesId == id, includeProperties: "Customer, Products");
            if (salesOrder == null)
            {
                return null;
            }
            _unitOfWork.DetachAllEntities();

            var products = updateSalesorderRequestDTO.Products.ToList();

            //Map DTO to Model
            var salesOrderModel = new SalesOrder
            {
                SalesId = id,
                SaleDate = DateTime.Now,
                QuantityForSale = updateSalesorderRequestDTO.QuantityForSale,
                TotalCostOfSalesOrder = updateSalesorderRequestDTO.TotalCostOfSalesOrder,
                SalesDiscount = updateSalesorderRequestDTO.SalesDiscount,
                CustomerId = updateSalesorderRequestDTO.CustomerId
            };

            //check if region exists
            salesOrderModel = await _unitOfWork.SalesOrder.Update(salesOrderModel);
            _unitOfWork.save();

            if (salesOrderModel == null)
            {
                return null;
            }

            //Convert Model to DTO
            var salesOrderDTO = new UpdateSalesOrderRequestDTO
            {
                SaleDate = DateTime.Now,
                QuantityForSale = salesOrderModel.QuantityForSale,
                TotalCostOfSalesOrder = salesOrderModel.TotalCostOfSalesOrder,
                SalesDiscount = salesOrderModel.SalesDiscount,
                CustomerId = salesOrderModel.CustomerId
            };
            return salesOrderDTO;
        }*/
    }
}