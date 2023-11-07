using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryEntities.Models
{
    public class Category
    {
        [Key]
        [DisplayName("Category Id")]
        public int CategoryId { get; set; }

        [Required]
        [DisplayName("Category Name")]
        [StringLength(100)]
        public string CategoryName { get; set; }

        //public IList<Product> Products { get; set; }
    }
}
