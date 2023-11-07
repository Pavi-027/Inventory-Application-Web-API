using InventoryDTO.DTO.Category_Dto;
using InventoryDTO.DTO.Customer_Dto;
using InventoryEntities.Models;

namespace InventoryServices.Services.ServiceInterface
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerDTO>> GetAll();
        Task<CustomerDTO> GetById(int id);
        Task<CustomerDTO> Create(AddCustomerRequestDTO customerDTO);
        Task<UpdateCustomerRequestDTO> Update(int id, UpdateCustomerRequestDTO customerDTO);
        Task<Customer> DeleteById(int id);
    }
}
