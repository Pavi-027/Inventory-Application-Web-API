using InventoryDTO.DTO.PurchaseOrder_Dto;
using InventoryDTO.DTO.Supplier_Dto;
using InventoryEntities.Models;
using InventoryRepositories.Repositories.Interfaces;
using InventoryServices.Services.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAPI.Controllers
{
    //[Authorize(Roles = "Admin, Manager")]
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISupplierService _supplierService;
        public SupplierController(IUnitOfWork unitOfWork, ISupplierService supplierService)
        {
            _unitOfWork = unitOfWork;
            _supplierService = supplierService;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var supplier = await _supplierService.GetAll();
            return Ok(supplier);
        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var supplier = await _supplierService.GetById(id);
            return Ok(supplier);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddSupplierRequestDTO addSupplierRequestDTO)
        {
            //Map or Convert DTO to Model
            var supplier = await _supplierService.Create(addSupplierRequestDTO);
            return Ok(supplier);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateSupplierRequestDTO updateSupplierRequestDTO)
        {
            var updatedSupplier = await _supplierService.Update(id, updateSupplierRequestDTO);
            return Ok(updatedSupplier);
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _supplierService.DeleteById(id);
            return Ok();
        }
    }
}
