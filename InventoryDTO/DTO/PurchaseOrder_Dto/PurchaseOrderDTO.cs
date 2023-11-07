using InventoryDTO.DTO.Product_Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryDTO.DTO.PurchaseOrder_Dto
{
    public class PurchaseOrderDTO
    {
        public int PurchaseId { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.Now;
        public int QuantityForPurchase { get; set; }
        public decimal TotalCostOfPurchaseorder { get; set; }
        public decimal? PurchaseDiscount { get; set; }
        public int SupplierId { get; set; }
        public int[] Products { get; set; }
    }
}
