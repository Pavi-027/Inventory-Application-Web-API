using InventoryDTO.DTO.PurchaseOrder_Dto;
using InventoryDTO.DTO.SalesOrder_Dto;
using InventoryEntities.Models;
using InventoryRepositories.Repositories.Interfaces;
using InventoryServices.Services.ServiceInterface;

namespace InventoryServices.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductService _productService;
        public PurchaseOrderService(IUnitOfWork unitOfWork, IProductService productService)
        {
            _unitOfWork = unitOfWork;
            _productService = productService;
        }

        public async Task<IEnumerable<PurchaseOrder>> GetAll()
        {
            var getAllPurchase = await _unitOfWork.PurchaseOrder.GetAll(includeProperties: "Supplier,Products");
            return getAllPurchase;
            /*var purchaseOrder = await _unitOfWork.PurchaseOrder.GetAll(includeProperties: "Supplier,Products");

            var purchaseOrderDTO = new List<PurchaseOrderDTO>();
            foreach (var item in purchaseOrder)
            {
                var products = item.Products.Select(p => p.ProductId).ToArray();

                purchaseOrderDTO.Add(new PurchaseOrderDTO()
                {
                    PurchaseId = item.PurchaseId,
                    PurchaseDate = DateTime.Now,
                    QuantityForPurchase = item.QuantityForPurchase,
                    TotalCostOfPurchaseorder = item.TotalCostOfPurchaseorder,
                    PurchaseDiscount = item.PurchaseDiscount,
                    SupplierId = item.SupplierId,
                    Products = products
                });
            }
            return purchaseOrderDTO;*/
        }
        public async Task<PurchaseOrderDTO> GetById(int id)
        {
            var purchaseOrder = await _unitOfWork.PurchaseOrder.GetById(x => x.PurchaseId == id, includeProperties: "Supplier,Products");

            if (purchaseOrder == null)
            {
                return null;
            }
            var products = purchaseOrder.Products.Select(p => p.ProductId).ToArray();
            
            var purchaseOrderDTO = new PurchaseOrderDTO
            {
                PurchaseId = purchaseOrder.PurchaseId,
                PurchaseDate = DateTime.Now,
                QuantityForPurchase = purchaseOrder.QuantityForPurchase,
                TotalCostOfPurchaseorder = purchaseOrder.TotalCostOfPurchaseorder,
                PurchaseDiscount = purchaseOrder.PurchaseDiscount,
                SupplierId = purchaseOrder.SupplierId,
                Products = products
            };
            return purchaseOrderDTO;
        }
        public async Task<PurchaseOrderResponseDTO> Create(AddPurchaseOrderRequestDTO addPurchaseOrderRequestDTO)
        {
            //Map or Convert DTO to Model
            var purchaseOrder = new PurchaseOrder()
            {
                PurchaseDate = DateTime.Now,
                QuantityForPurchase = addPurchaseOrderRequestDTO.QuantityForPurchase,
                TotalCostOfPurchaseorder = addPurchaseOrderRequestDTO.TotalCostOfPurchaseorder,
                PurchaseDiscount = addPurchaseOrderRequestDTO.PurchaseDiscount,
                SupplierId = addPurchaseOrderRequestDTO.SupplierId
            };

            var existingProduct = await _productService.GetByIds(addPurchaseOrderRequestDTO.Products);

            purchaseOrder.Products = existingProduct;

            //Use Model to Create Region
            purchaseOrder = await _unitOfWork.PurchaseOrder.Add(purchaseOrder);
            _unitOfWork.save();

            //Map Model Back to DTO
            var purchaseOrderDTO = new PurchaseOrderResponseDTO
            {
                PurchaseDate = DateTime.Now,
                QuantityForPurchase = purchaseOrder.QuantityForPurchase,
                TotalCostOfPurchaseorder = purchaseOrder.TotalCostOfPurchaseorder,
                PurchaseDiscount = purchaseOrder.PurchaseDiscount,
                SupplierId = purchaseOrder.SupplierId,
                Products = purchaseOrder.Products
            };
            return purchaseOrderDTO;
        }

        public async Task<PurchaseOrder> DeleteById(int id)
        {
            PurchaseOrder deletePurchaseOrderById = await _unitOfWork.PurchaseOrder.GetById(x => x.PurchaseId == id);

            if (deletePurchaseOrderById == null)
            {
                return null;
            }
            await _unitOfWork.PurchaseOrder.Delete(deletePurchaseOrderById);
            _unitOfWork.save();
            return deletePurchaseOrderById;
        }
    }
}
