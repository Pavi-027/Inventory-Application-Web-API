using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryDTO.DTO.Customer_Dto
{
    public class UpdateCustomerRequestDTO
    {
        public string CustomerName { get; set; }
        public long PhoneNumber { get; set; }
        public string EmailId { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public long Pincode { get; set; }
    }
}
