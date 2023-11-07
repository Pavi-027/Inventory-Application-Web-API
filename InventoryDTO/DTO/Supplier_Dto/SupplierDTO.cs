using InventoryDTO.DTO.PurchaseOrder_Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryDTO.DTO.Supplier_Dto
{
    public class SupplierDTO
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public long PhoneNumber { get; set; }
        public string EmailId { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public long Pincode { get; set; }
        public ICollection<PurchaseOrderDTO> PurchaseOrders { get; set; }
    }
}
