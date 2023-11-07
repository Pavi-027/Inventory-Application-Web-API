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
    public class PurchaseOrder
    {
        [Key]
        [Display(Name = "Purchase Id")]
        public int PurchaseId { get; set; }

        [Required]
        [Display(Name = "Purchase Date")]
        public DateTime PurchaseDate { get; set; } = DateTime.Now;

        [Required]
        public int QuantityForPurchase { get; set; }

        [Required]
        [Display(Name = "Total Cost")]
        public decimal TotalCostOfPurchaseorder { get; set; }

        public decimal? PurchaseDiscount { get; set; }

        [Required]
        [ForeignKey(nameof(Supplier))]
        [ValidateNever]
        public int SupplierId { get; set; }
        [ValidateNever]
        public virtual Supplier Supplier { get; set; }

        [ValidateNever]
        public ICollection<Product> Products { get; set; }
    }
}
