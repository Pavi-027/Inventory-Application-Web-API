using InventoryDTO.DTO.Product_Dto;
using InventoryDTO.DTO.SalesOrder_Dto;
using InventoryEntities.Models;
using InventoryRepositories.Repositories.Interfaces;
using InventoryServices.Services.ServiceInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesOrderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISalesOrderService _salesOrderService;
        public SalesOrderController(IUnitOfWork unitOfWork, ISalesOrderService salesOrderService)
        {
            _unitOfWork = unitOfWork;
            _salesOrderService = salesOrderService;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var supplier = await _salesOrderService.GetAll();
            return Ok(supplier);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var salesOrder = await _salesOrderService.GetById(id);
            return Ok(salesOrder);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddSalesorderRequestDTO addSalesOrderRequestDTO)
        {
            var salesOrder = await _salesOrderService.Create(addSalesOrderRequestDTO);
            return Ok(salesOrder);
        }

        /*[HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateSalesOrderRequestDTO updateSalesorderRequestDTO)
        {
            var updatedSalesOrder = await _salesOrderService.Update(id, updateSalesorderRequestDTO);
            return Ok(updatedSalesOrder);
        }*/
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _salesOrderService.DeleteById(id);
            return Ok();
        }
    }
}
