using InventoryEntities.IdentityEntities;
using InventoryRepositories.DataAccess;
using InventoryUtility;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InventoryServices.Services
{
    public class ContextSeedService
    {
       private readonly InventoryDbContext _dbcontext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        //Context context,
        public ContextSeedService(InventoryDbContext dbcontext,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _dbcontext = dbcontext;
            _userManager = userManager;
            _roleManager = roleManager;            
        }

        public async Task InitializeContextAsync()
        {
            if(_dbcontext.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Count() > 0)
            {
                //applies any pending migration into our database
                await _dbcontext.Database.MigrateAsync();
            }

            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = SD.AdminRole });
                await _roleManager.CreateAsync(new IdentityRole { Name = SD.ManagerRole });
                await _roleManager.CreateAsync(new IdentityRole { Name = SD.SalesPersonRole });
            }

            if (!_userManager.Users.AnyAsync().GetAwaiter().GetResult())
            {
                var admin = new ApplicationUser
                {
                    FullName = "admin",
                    UserName = SD.AdminUserName,
                    Email = SD.AdminUserName,
                    EmailConfirmed = true,
                    StreetAddress = "abc street",
                    City = "abc",
                    State = "abc",
                    Pincode = 635607
                };
                admin.CreatedBy = admin.Id;
                admin.UpdatedBy = admin.Id;

                await _userManager.CreateAsync(admin, "Admin@123");
                await _userManager.AddToRolesAsync(admin, new[] { SD.AdminRole, SD.ManagerRole, SD.SalesPersonRole });
                await _userManager.AddClaimsAsync(admin, new Claim[]
                {
                    new Claim(ClaimTypes.Email, admin.Email),
                    new Claim(ClaimTypes.GivenName, admin.FullName)
                });

                var manager = new ApplicationUser
                {
                    FullName = "manager",
                    UserName = "manager@gmail.com",
                    Email = "manager@gmail.com",
                    EmailConfirmed = true,
                    StreetAddress = "xyz street",
                    City = "xyz",
                    State = "xyz",
                    Pincode = 637847
                };
                manager.CreatedBy = manager.Id;
                manager.UpdatedBy = manager.Id;

                await _userManager.CreateAsync(manager, "Manager@123");
                await _userManager.AddToRoleAsync(manager, SD.ManagerRole);
                await _userManager.AddClaimsAsync(manager, new Claim[]
                {
                    new Claim(ClaimTypes.Email, manager.Email),
                    new Claim(ClaimTypes.GivenName, manager.FullName)
                });

                var salesPerson1 = new ApplicationUser
                {
                    FullName = "salesPerson1",
                    UserName = "salesPerson1@gmail.com",
                    Email = "salesPerson1@gmail.com",
                    EmailConfirmed = true,
                    StreetAddress = "pqr street",
                    City = "pqr",
                    State = "pqr",
                    Pincode = 600647
                };
                salesPerson1.CreatedBy = salesPerson1.Id;
                salesPerson1.UpdatedBy = salesPerson1.Id;

                await _userManager.CreateAsync(salesPerson1, "Salesperson1@123");
                await _userManager.AddToRoleAsync(salesPerson1, SD.SalesPersonRole);
                await _userManager.AddClaimsAsync(salesPerson1, new Claim[]
                {
                    new Claim(ClaimTypes.Email, salesPerson1.Email),
                    new Claim(ClaimTypes.GivenName, salesPerson1.FullName)
                });

                var salesPerson2 = new ApplicationUser
                {
                    FullName = "salesPerson2",
                    UserName = "salesPerson2@gmail.com",
                    Email = "salesPerson2@gmail.com",
                    EmailConfirmed = true,
                    StreetAddress = "mno street",
                    City = "mno",
                    State = "mno",
                    Pincode = 600647
                };
                salesPerson2.CreatedBy = salesPerson2.Id;
                salesPerson2.UpdatedBy = salesPerson2.Id;

                await _userManager.CreateAsync(salesPerson2, "Salesperson2@123");
                await _userManager.AddToRoleAsync(salesPerson2, SD.SalesPersonRole);
                await _userManager.AddClaimsAsync(salesPerson2, new Claim[]
                {
                    new Claim(ClaimTypes.Email, salesPerson2.Email),
                    new Claim(ClaimTypes.GivenName, salesPerson2.FullName)
                });
            }
        }

    }
}
