using InventoryDTO.DTO.PurchaseOrder_Dto;
using InventoryDTO.DTO.SalesOrder_Dto;
using InventoryEntities.Models;

namespace InventoryServices.Services.ServiceInterface
{
    public interface IPurchaseOrderService
    {
        Task<IEnumerable<PurchaseOrder>> GetAll();
        Task<PurchaseOrderDTO> GetById(int id);
        Task<PurchaseOrderResponseDTO> Create(AddPurchaseOrderRequestDTO purchaseOrderDTO);
        //Task<UpdateSalesOrderRequestDTO> Update(int id, UpdateSalesOrderRequestDTO salesOrderDTO);
        Task<PurchaseOrder> DeleteById(int id);
    }
}
