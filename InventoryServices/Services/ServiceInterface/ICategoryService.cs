
using InventoryDTO.DTO.Category_Dto;
using InventoryEntities.Models;

namespace InventoryServices.Services.ServiceInterface
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAll();
        Task<CategoryDTO> GetById(int id);
        Task<AddCategoryRequestDTO> Create(AddCategoryRequestDTO categoryDTO);
        Task<UpdateCategoryRequestDTO> Update(int id, UpdateCategoryRequestDTO categoryDTO);
        Task<Category> DeleteById(int id);

    }
}
