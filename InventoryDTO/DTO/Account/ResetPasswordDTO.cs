using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryDTO.DTO.Account
{
    public class ResetPasswordDTO
    {
        [Required]
        public string Token { get; set; }
        [Required]
        [RegularExpression("^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$", ErrorMessage = "Invalid Email Id")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "New Password must be atleast {3}, and maximum {1} characters")]
        public string NewPassword { get; set; }

    }
}
