using InventoryDTO.DTO;
using InventoryDTO.DTO.Product_Dto;
using InventoryEntities.Models;
using InventoryRepositories.Repositories.Interfaces;
using InventoryServices.Services.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAPI.Controllers
{
    //[Authorize(Roles = "Admin, Manager, SalesPerson")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductService _productService;
        public ProductController(IUnitOfWork unitOfWork, IProductService productService)
        {
            _unitOfWork = unitOfWork;
            _productService = productService;
        }
        //GET ALL REGIONS
        //GET: http://localhost:portnumber/api/category
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var product = await _productService.GetAll();
            return Ok(product);
        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var product = await _productService.GetById(id);
            return Ok(product);
        }
        /*[HttpGet]
        [Route("{ids:int[]}")]
        public async Task<IActionResult> GetByIds([FromRoute] int[] id)
        {
            var productByIds = await _productService.GetByIds(id);
            return Ok(productByIds);
        }*/
        [HttpPost]
        public async Task<IActionResult> Create( [FromBody] AddProductRequestDTO addProductDTO)
        {
            var product = await _productService.Create(addProductDTO);
            return Ok(product);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateProductRequestDTO updateProductRequestDTO)
        {
            var updatedProduct = await _productService.Update(id, updateProductRequestDTO);
            return Ok(updatedProduct);
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _productService.DeleteById(id);
            return Ok();
        }
    }
}
