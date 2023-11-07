using InventoryDTO.DTO.Category_Dto;
using InventoryDTO.DTO.Customer_Dto;
using InventoryDTO.DTO.Supplier_Dto;
using InventoryEntities.Models;

namespace InventoryServices.Services.ServiceInterface
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierDTO>> GetAll();
        Task<SupplierDTO> GetById(int id);
        Task<AddSupplierRequestDTO> Create(AddSupplierRequestDTO supplierDTO);
        Task<UpdateSupplierRequestDTO> Update(int id, UpdateSupplierRequestDTO supplierDTO);
        Task<Supplier> DeleteById(int id);
    }
}
