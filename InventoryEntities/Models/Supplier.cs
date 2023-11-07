using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InventoryEntities.Models
{
    public class Supplier
    {
        [Key]
        [Display(Name = "Supplier Id")]
        public int SupplierId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Supplier Name")]
        public string SupplierName { get; set; }

        [Required]
        [RegularExpression("^([0-9]{10})$", ErrorMessage = "Please Enter a Valid Mobile Number")]
        [Display(Name = "Phone Number")]
        public long PhoneNumber { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_\-\.]+[@][a-z]+[\.][a-z]{2,3}$", ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email Id")]
        public string EmailId { get; set; }

        [Required]
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "PIN code must be exactly 6 characters.")]
        public long Pincode { get; set; }

        [ValidateNever]
        [JsonIgnore]
        public ICollection<PurchaseOrder> PurchaseOrders { get; set; }
    }
}
