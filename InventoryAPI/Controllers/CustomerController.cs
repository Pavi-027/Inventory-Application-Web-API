using InventoryDTO.DTO.Customer_Dto;
using InventoryDTO.DTO.SalesOrder_Dto;
using InventoryEntities.Models;
using InventoryRepositories.Repositories.Interfaces;
using InventoryServices.Services.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace InventoryAPI.Controllers
{
    //[Authorize(Roles = "SalesPerson")]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerService _customerService;
        public CustomerController(IUnitOfWork unitOfWork, ICustomerService customerService)
        {
            _unitOfWork = unitOfWork;
            _customerService = customerService;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var customer = await _customerService.GetAll();
            return Ok(customer);
        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var customer = await _customerService.GetById(id);
            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddCustomerRequestDTO addCustomerRequestDTO)
        {
            var customer = await _customerService.Create(addCustomerRequestDTO);
            return Ok(customer);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCustomerRequestDTO updateCustomerRequestDTO)
        {
            var updatedCustomer = await _customerService.Update(id, updateCustomerRequestDTO);
            return Ok(updatedCustomer);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _customerService.DeleteById(id);
            return Ok();
        }

    }
}
