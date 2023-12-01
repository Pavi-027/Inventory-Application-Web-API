using InventoryEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryDTO.DTO.Product_Dto
{
    public class ProductRequestDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        public int TotalQuantityOfProduct { get; set; }
        public int QuantityInstock { get; set; }
        public string Status { get; set; }
        //public int CategoryId { get; set; }
        public Category Category { get; set; }
       
    }
}
