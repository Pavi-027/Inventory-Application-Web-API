using InventoryDTO.DTO.Category_Dto;
using InventoryEntities.Models;
using InventoryRepositories.Repositories.Interfaces;
using InventoryServices.Services.ServiceInterface;

namespace InventoryServices.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<CategoryDTO>> GetAll()
        {
            var category = await _unitOfWork.Category.GetAll();

            //Map Models to DTOs
            var categoryDTO = new List<CategoryDTO>();
            foreach (var item in category)
            {
                categoryDTO.Add(new CategoryDTO()
                {
                    CategoryId = item.CategoryId,
                    CategoryName = item.CategoryName
                });
            }
            //Return DTOs
            return categoryDTO;
        }

        public async Task<CategoryDTO> GetById(int id)
        {
            var category = await _unitOfWork.Category.GetById(x => x.CategoryId == id);

            if (category == null)
            {
                return null;
            }

            var categoryDTO = new CategoryDTO
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName
            };

            return categoryDTO;
        }
        public async Task<AddCategoryRequestDTO> Create(AddCategoryRequestDTO addCategoryRequestDTO)
        {
            var category = new Category()
            {
                CategoryName = addCategoryRequestDTO.CategoryName
            };

            //Use Model to Create Region
            category = await _unitOfWork.Category.Add(category);
            _unitOfWork.save();

            //Map Model Back to DTO
            var categoryDTO = new AddCategoryRequestDTO
            {
                CategoryName = category.CategoryName
            };
            return categoryDTO;
        }
        public async Task<UpdateCategoryRequestDTO> Update(int id, UpdateCategoryRequestDTO updateCategoryRequestDTO)
        {
            var category = _unitOfWork.Category.GetById(x => x.CategoryId == id);
            if (category == null)
            {
                return null;
            }
            _unitOfWork.DetachAllEntities();

            //Map DTO to Model
            var categoryModel = new Category
            {
                CategoryId = id,
                CategoryName = updateCategoryRequestDTO.CategoryName
            };

            //check if region exists
            categoryModel = await _unitOfWork.Category.Update(categoryModel);
            _unitOfWork.save();

            if (categoryModel == null)
            {
                return null;
            }

            //Convert Model to DTO
            var categoryDTO = new UpdateCategoryRequestDTO
            {
                CategoryName = categoryModel.CategoryName
            };
            return categoryDTO;
        }

        public async Task<Category> DeleteById(int id)
        {
            Category deleteCategoryById = await _unitOfWork.Category.GetById(x => x.CategoryId == id);

            if (deleteCategoryById == null)
            {
                return null;
            }
            await _unitOfWork.Category.Delete(deleteCategoryById);
            _unitOfWork.save();
            return deleteCategoryById;
        }
    }
}
