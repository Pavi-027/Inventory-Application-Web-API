using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryEntities.IdentityEntities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public long? Pincode { get; set; }

        public string CreatedBy { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public virtual ApplicationUser CreatedByUser { get; set; }

        public DateTimeOffset CreatedOn { get; set; } = DateTime.Now;

        public string UpdatedBy { get; set; }

        [ForeignKey(nameof(UpdatedBy))]
        public virtual ApplicationUser UpdatedByUser { get; set; }

        public DateTimeOffset UpdatedOn { get; set; } = DateTime.Now;

    }
}
