using InventoryDTO.DTO.PurchaseOrder_Dto;
using InventoryEntities.Models;
using InventoryRepositories.Repositories.Interfaces;
using InventoryServices.Services.ServiceInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPurchaseOrderService _purchaseOrderService;
        public PurchaseOrderController(IUnitOfWork unitOfWork, IPurchaseOrderService purchaseOrderService)
        {
            _unitOfWork = unitOfWork;
            _purchaseOrderService = purchaseOrderService;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var purchaseOrder = await _purchaseOrderService.GetAll();
            return Ok(purchaseOrder);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var purchaseOrder = await _purchaseOrderService.GetById(id);
            return Ok(purchaseOrder);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddPurchaseOrderRequestDTO addPurchaseOrderRequestDTO)
        {
            var purchaseOrder = await _purchaseOrderService.Create(addPurchaseOrderRequestDTO);
            return Ok(purchaseOrder);
        }

        /*[HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdatePurchaseOrderRequestDTO updatePurchaseorderRequestDTO)
        {
            var purchaseOrder = _unitOfWork.PurchaseOrder.GetById(x => x.PurchaseId == id);
            if (purchaseOrder == null)
            {
                return NotFound();
            }
            _unitOfWork.DetachAllEntities();

            //Map DTO to Model
            var purchaseOrderModel = new PurchaseOrder
            {
                PurchaseId = id,
                PurchaseDate = DateTime.Now,
                QuantityForPurchase = updatePurchaseorderRequestDTO.QuantityForPurchase,
                TotalCostOfPurchaseorder = updatePurchaseorderRequestDTO.TotalCostOfPurchaseorder,
                PurchaseDiscount = updatePurchaseorderRequestDTO.PurchaseDiscount,
                SupplierId = updatePurchaseorderRequestDTO.SupplierId
            };

            //check if region exists
            purchaseOrderModel = await _unitOfWork.PurchaseOrder.Update(purchaseOrderModel);
            _unitOfWork.save();

            if (purchaseOrderModel == null)
            {
                return NotFound();
            }

            //Convert Model to DTO
            var purchaseOrderDTO = new PurchaseOrderDTO
            {
                PurchaseId = id,
                PurchaseDate = DateTime.Now,
                QuantityForPurchase = purchaseOrderModel.QuantityForPurchase,
                TotalCostOfPurchaseorder = purchaseOrderModel.TotalCostOfPurchaseorder,
                PurchaseDiscount = purchaseOrderModel.PurchaseDiscount,
                SupplierId = purchaseOrderModel.SupplierId
            };
            return Ok(purchaseOrderDTO);
        }*/
        
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _purchaseOrderService.DeleteById(id);
            return Ok();
        }
    }
}
