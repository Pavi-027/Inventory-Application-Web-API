using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryDTO.DTO.Admin
{
    public class MemberAddEditDTO
    {
        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string FullName { get; set; }

        public string Password { get; set; }

        [Required]
        public string Roles { get; set; }
    }
}
