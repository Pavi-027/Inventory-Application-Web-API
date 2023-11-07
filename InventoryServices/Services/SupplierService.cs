using InventoryDTO.DTO.PurchaseOrder_Dto;
using InventoryDTO.DTO.Supplier_Dto;
using InventoryEntities.Models;
using InventoryRepositories.Repositories.Interfaces;
using InventoryServices.Services.ServiceInterface;

namespace InventoryServices.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SupplierService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;            
        }

        public async Task<IEnumerable<SupplierDTO>> GetAll()
        {
            var supplier = await _unitOfWork.Supplier.GetAll(includeProperties: "PurchaseOrders");

            var supplierDTO = new List<SupplierDTO>();
            foreach (var item in supplier)
            {
                supplierDTO.Add(new SupplierDTO()
                {
                    SupplierId = item.SupplierId,
                    SupplierName = item.SupplierName,
                    EmailId = item.EmailId,
                    PhoneNumber = item.PhoneNumber,
                    StreetAddress = item.StreetAddress,
                    City = item.City,
                    State = item.State,
                    Pincode = item.Pincode,
                    PurchaseOrders = item.PurchaseOrders.Select(x => new PurchaseOrderDTO
                    {
                        PurchaseId = x.PurchaseId,
                        PurchaseDate = x.PurchaseDate,
                        QuantityForPurchase = x.QuantityForPurchase,
                        TotalCostOfPurchaseorder = x.TotalCostOfPurchaseorder,
                        PurchaseDiscount = x.PurchaseDiscount,
                        SupplierId = x.SupplierId
                    }).ToList()
                });
            }
            return supplierDTO;
        }

        public async Task<SupplierDTO> GetById(int id)
        {
            var supplier = await _unitOfWork.Supplier.GetById(x => x.SupplierId == id, includeProperties: "PurchaseOrders");

            if (supplier == null)
            {
                return null;
            }
            var supplierDTO = new SupplierDTO
            {
                SupplierId = supplier.SupplierId,
                SupplierName = supplier.SupplierName,
                EmailId = supplier.EmailId,
                PhoneNumber = supplier.PhoneNumber,
                StreetAddress = supplier.StreetAddress,
                City = supplier.City,
                State = supplier.State,
                Pincode = supplier.Pincode,
                PurchaseOrders = supplier.PurchaseOrders.Select(x => new PurchaseOrderDTO
                {
                    PurchaseId = x.PurchaseId,
                    PurchaseDate = x.PurchaseDate,
                    QuantityForPurchase = x.QuantityForPurchase,
                    TotalCostOfPurchaseorder = x.TotalCostOfPurchaseorder,
                    PurchaseDiscount = x.PurchaseDiscount,
                    SupplierId = x.SupplierId
                }).ToList()
            };
            return supplierDTO;
        }
        public async Task<AddSupplierRequestDTO> Create(AddSupplierRequestDTO addSupplierRequestDTO)
        {
            //Map or Convert DTO to Model
            var supplier = new Supplier()
            {
                SupplierName = addSupplierRequestDTO.SupplierName,
                EmailId = addSupplierRequestDTO.EmailId,
                PhoneNumber = addSupplierRequestDTO.PhoneNumber,
                StreetAddress = addSupplierRequestDTO.StreetAddress,
                City = addSupplierRequestDTO.City,
                State = addSupplierRequestDTO.State,
                Pincode = addSupplierRequestDTO.Pincode,
            };

            //Use Model to Create Region
            supplier = await _unitOfWork.Supplier.Add(supplier);
            _unitOfWork.save();

            //Map Model Back to DTO
            var supplierDTO = new AddSupplierRequestDTO
            {
                SupplierName = supplier.SupplierName,
                EmailId = supplier.EmailId,
                PhoneNumber = supplier.PhoneNumber,
                StreetAddress = supplier.StreetAddress,
                City = supplier.City,
                State = supplier.State,
                Pincode = supplier.Pincode
            };
            return supplierDTO;
        }

        public async Task<Supplier> DeleteById(int id)
        {
            Supplier deleteSupplierById = await _unitOfWork.Supplier.GetById(x => x.SupplierId == id);

            if (deleteSupplierById == null)
            {
                return null;
            }
            await _unitOfWork.Supplier.Delete(deleteSupplierById);
            _unitOfWork.save();
            return deleteSupplierById;
        }

        public async Task<UpdateSupplierRequestDTO> Update(int id, UpdateSupplierRequestDTO updateSupplierRequestDTO)
        {
            var supplier = _unitOfWork.Supplier.GetById(x => x.SupplierId == id, includeProperties: "PurchaseOrders");
            if (supplier == null)
            {
                return null;
            }
            _unitOfWork.DetachAllEntities();

            //Map DTO to Model
            var supplierModel = new Supplier
            {
                SupplierId = id,
                SupplierName = updateSupplierRequestDTO.SupplierName,
                EmailId = updateSupplierRequestDTO.EmailId,
                PhoneNumber = updateSupplierRequestDTO.PhoneNumber,
                StreetAddress = updateSupplierRequestDTO.StreetAddress,
                City = updateSupplierRequestDTO.City,
                State = updateSupplierRequestDTO.State,
                Pincode = updateSupplierRequestDTO.Pincode
            };

            //check if region exists
            supplierModel = await _unitOfWork.Supplier.Update(supplierModel);
            _unitOfWork.save();

            if (supplierModel == null)
            {
                return null;
            }

            //Convert Model to DTO
            var supplierDTO = new UpdateSupplierRequestDTO
            {
                SupplierName = supplierModel.SupplierName,
                EmailId = supplierModel.EmailId,
                PhoneNumber = supplierModel.PhoneNumber,
                StreetAddress = supplierModel.StreetAddress,
                City = supplierModel.City,
                State = supplierModel.State,
                Pincode = supplierModel.Pincode,
            };
            return supplierDTO;
        }
    }
}
