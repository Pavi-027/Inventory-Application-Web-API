using InventoryDTO.DTO.Category_Dto;
using InventoryEntities.Models;
using InventoryRepositories.DataAccess;
using InventoryRepositories.Repositories.Interfaces;
using InventoryServices.Services.ServiceInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryService _categoryService;
        public CategoryController(IUnitOfWork unitOfWork, ICategoryService categoryService)
        {
            _unitOfWork = unitOfWork;
            _categoryService = categoryService;
        }

        //GET ALL REGIONS
        //GET: http://localhost:portnumber/api/category
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var category = await _categoryService.GetAll();
            return Ok(category);
        }

        //GET SINGLE REGIONS
        //GET: http://localhost:portnumber/api/category/{id}
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var categoryById = await _categoryService.GetById(id);
            return Ok(categoryById);
        }

        //POST To Create New Categpry
        //POST: http://localhost:portnumber/api/category
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddCategoryRequestDTO addCategoryRequestDTO)
        {
            var category = await _categoryService.Create(addCategoryRequestDTO);

            return Ok(category);
        }

        //PUT To Update One Categpry
        //PUT: http://localhost:portnumber/api/category/id
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCategoryRequestDTO updateCategoryRequestDTO)
        {
            var updatedCategory = await _categoryService.Update(id, updateCategoryRequestDTO);
            return Ok(updatedCategory);
        }

        //DELETE Category
        //DELETE: http://localhost:portnumber/api/category/id
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            _categoryService.DeleteById(id);
            return Ok();
        }
    }
}
