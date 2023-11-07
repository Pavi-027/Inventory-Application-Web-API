using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryUtility
{
    public static class SD
    {
        //Roles
        public const string AdminRole = "Admin";
        public const string ManagerRole = "Manager";
        public const string SalesPersonRole = "SalesPerson";

        public const string AdminUserName = "admin@gmail.com";
        public const string SuperAdminChangeNotallowed = "Super Admin change is not allowed";

        public const int MaximumLoginAttempts = 3;
    }
}
