using InventoryDTO.DTO.Category_Dto;
using InventoryDTO.DTO.Product_Dto;
using InventoryEntities.Models;

namespace InventoryServices.Services.ServiceInterface
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAll();
        Task<ProductRequestDto> GetById(int id);
        Task<List<Product>> GetByIds(int[] ids);
        Task<AddProductRequestDTO> Create(AddProductRequestDTO productDTO);
        Task<UpdateProductRequestDTO> Update(int id, UpdateProductRequestDTO productDTO);
        Task<Product> DeleteById(int id);
    }
}
