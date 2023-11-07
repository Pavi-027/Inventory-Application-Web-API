using InventoryDTO.DTO.Category_Dto;
using InventoryDTO.DTO.Customer_Dto;
using InventoryDTO.DTO.SalesOrder_Dto;
using InventoryEntities.Models;

namespace InventoryServices.Services.ServiceInterface
{
    public interface ISalesOrderService
    {
        Task<IEnumerable<SalesOrder>> GetAll();
        Task<SalesOrderDTO> GetById(int id);
        Task<SalesOrderResponseDTO> Create(AddSalesorderRequestDTO salesOrderDTO);
        //Task<UpdateSalesOrderRequestDTO> Update(int id, UpdateSalesOrderRequestDTO salesOrderDTO);
        Task<SalesOrder> DeleteById(int id);
    }
}
