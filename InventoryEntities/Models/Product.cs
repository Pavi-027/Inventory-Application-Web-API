using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;

namespace InventoryEntities.Models
{
    public class Product
    {
        [Key]
        [Display(Name = "Product Id")]
        public int ProductId { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Required]
        public string? Description { get; set; }

        [Display(Name = "Product Image")]
        [ValidateNever]
        public string? ProductImageURL { get; set; }

        [Required]
        public decimal Price { get; set; }

        public decimal? Discount { get; set; }

        [Required]
        [Display(Name = "Total Quantity of a Product")]
        public int TotalQuantityOfProduct { get; set; }

        [Required]
        [Display(Name = "Quantity In Stock")]
        public int QuantityInstock { get; set; }

        [Required]
        public string Status { get; set; }

        [ForeignKey(nameof(Category))]
        [ValidateNever]
        [JsonIgnore]
        public int CategoryId { get; set; }
        [ValidateNever]
        [JsonIgnore]
        public virtual Category Category { get; set; }

        [ValidateNever]
        [JsonIgnore]
        public ICollection<SalesOrder> Sales { get; set; }

        [ValidateNever]
        [JsonIgnore]
        public ICollection<PurchaseOrder> Purchase { get; set; }
    }
    /*public enum Status
    {
        InStock,
        Sold
    }*/
}
