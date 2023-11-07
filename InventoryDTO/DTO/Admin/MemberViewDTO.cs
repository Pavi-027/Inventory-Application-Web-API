using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryDTO.DTO.Admin
{
    public class MemberViewDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public bool IsLocked { get; set; }
        public DateTimeOffset DateCreated { get; set; } = DateTime.Now;
        public IEnumerable<string> Roles { get; set; }
    }
}
