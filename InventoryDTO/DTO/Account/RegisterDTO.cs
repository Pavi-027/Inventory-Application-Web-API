using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryDTO.DTO.Account
{
    public class RegisterDTO
    {
        //public string Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be atleast {2} and maximum {1} characters")]
        public string FullName {  get; set; }

        [Required]
        [RegularExpression("^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$", ErrorMessage = "Invalid Email Id")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "Password must be atleast {3}, and maximum {1} characters")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public long? Pincode { get; set; }
    }
}
