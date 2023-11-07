using InventoryDTO.DTO.Product_Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryDTO.DTO.SalesOrder_Dto
{
    public class UpdateSalesOrderRequestDTO
    {
        public DateTime SaleDate { get; set; } = DateTime.Now;
        public int QuantityForSale { get; set; }
        public decimal TotalCostOfSalesOrder { get; set; }
        public decimal? SalesDiscount { get; set; }
        public int CustomerId { get; set; }
        public int[] Products { get; set; }
    }
}
