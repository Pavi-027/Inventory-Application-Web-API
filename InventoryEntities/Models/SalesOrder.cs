using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InventoryEntities.Models
{
    public class SalesOrder
    {
        [Key]
        [Display(Name = "Sales Id")]
        public int SalesId { get; set; }

        [Required]
        [Display(Name = "Sale Date")]
        public DateTime SaleDate { get; set; } = DateTime.Now;

        [Required]
        public int QuantityForSale { get; set; }

        [Required]
        [Display(Name = "Total Cost")]
        public decimal TotalCostOfSalesOrder { get; set; }

        public decimal? SalesDiscount { get; set; }

        [Required]
        [ForeignKey(nameof(Customer))]
        [ValidateNever]
        public int CustomerId { get; set; }
        [ValidateNever]
        public virtual Customer Customer { get; set; }

        [ValidateNever]
        public ICollection<Product> Products { get; set; }
    }
}
