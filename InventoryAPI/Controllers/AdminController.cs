using InventoryDTO.DTO.Admin;
using InventoryEntities.IdentityEntities;
using InventoryUtility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AdminController(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /*[HttpGet]
        [Route("get-members")]
        public async Task<ActionResult<IEnumerable<MemberViewDTO>>> GetAllMembers()
        {
            var members = await _userManager.Users
                .Where(x => x.UserName != "admin@gmail.com")
                .Select(member => new MemberViewDTO
                {
                    Id = member.Id,
                    UserName = member.UserName,
                    FullName = member.FullName,
                    DateCreated = member.CreatedOn,
                    IsLocked = _userManager.IsLockedOutAsync(member).GetAwaiter().GetResult(),
                    Roles = _userManager.GetRolesAsync(member).GetAwaiter().GetResult()
                }).ToListAsync();

            return Ok(members);
        }*/

        [HttpGet]
        [Route("get-members")]
        public async Task<ActionResult<IEnumerable<MemberViewDTO>>> GetAllMembers()
        {
            var users = await _userManager.Users
                .Where(x => x.UserName != SD.AdminUserName)
                .ToListAsync();

            var members = new List<MemberViewDTO>();

            foreach (var user in users)
            {
                var isLocked = await _userManager.IsLockedOutAsync(user);
                var roles = await _userManager.GetRolesAsync(user);

                members.Add(new MemberViewDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    DateCreated = user.CreatedOn,
                    IsLocked = isLocked,
                    Roles = roles
                });
            }
            return Ok(members);
        }

        [HttpGet]
        [Route("get-member/{id}")]
        public async Task<ActionResult<MemberAddEditDTO>> GetMemeber(string id)
        {
            var user = await _userManager.Users
                .Where(x => x.UserName != SD.AdminUserName && x.Id == id)
                .FirstOrDefaultAsync();

            if (user == null) return NotFound();

            var roles = string.Join(",", await _userManager.GetRolesAsync(user));

            var member = new MemberAddEditDTO { 
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Roles = roles
            };
            return Ok(member);
        }

        [HttpPost]
        [Route("add-edit-member")]
        public async Task<IActionResult> AddEditMember(MemberAddEditDTO model)
        {
            ApplicationUser user;

            if (string.IsNullOrEmpty(model.Id))
            {
                // adding a new member
                if(string.IsNullOrEmpty(model.Password) || model.Password.Length < 6)
                {
                    ModelState.AddModelError("errors", "Password must be atleast 6 characters");
                    return BadRequest(ModelState);
                }

                user = new ApplicationUser
                {
                    FullName = model.FullName,
                    UserName = model.UserName,
                    EmailConfirmed = true
                };
                user.CreatedBy = user.Id;
                user.UpdatedBy = user.Id;

                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded) return BadRequest(result.Errors);
            }
            else
            {
                // editing a existing member
                if (!string.IsNullOrEmpty(model.Password))
                {
                    if (model.Password.Length < 6)
                    {
                        ModelState.AddModelError("errors", "Password must be atleast 6 characters");
                        return BadRequest(ModelState);
                    }
                }

                if (IsAdminUserId(model.Id))
                {
                    return BadRequest(SD.SuperAdminChangeNotallowed);
                }

                user = await _userManager.FindByIdAsync(model.Id);
                if (user == null) return NotFound();

                user.FullName = model.FullName;
                user.UserName = model.UserName;

                if (!string.IsNullOrEmpty(model.Password))
                {
                    await _userManager.RemovePasswordAsync(user);
                    await _userManager.AddPasswordAsync(user, model.Password);
                }
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            //removing users existing roles
            await _userManager.RemoveFromRolesAsync(user, userRoles);

            foreach(var role in model.Roles.Split(",").ToArray())
            {
                var roleToAdd = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == role);
                if(roleToAdd != null)
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }

            if (string.IsNullOrEmpty(model.Id))
            {
                return Ok(new JsonResult(new { title = "Member Created", message = $"{model.UserName} has been created" }));
            }
            else
            {
                return Ok(new JsonResult(new { title = "Member Edited", message = $"{model.UserName} has been updated" }));
            }
        }


        [HttpPut]
        [Route("lock-member/{id}")]
        public async Task<IActionResult> LockMember(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (IsAdminUserId(id))
            {
                return BadRequest(SD.SuperAdminChangeNotallowed);
            }

            await _userManager.SetLockoutEndDateAsync(user, DateTime.UtcNow.AddDays(5));
            return NoContent();
        }


        [HttpPut]
        [Route("unlock-member/{id}")]
        public async Task<IActionResult> UnlockMember(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (IsAdminUserId(id))
            {
                return BadRequest(SD.SuperAdminChangeNotallowed);
            }

            await _userManager.SetLockoutEndDateAsync(user, null);
            return NoContent();
        }

        [HttpDelete]
        [Route("delete-member/{id}")]
        public async Task<IActionResult> DeleteMember(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (IsAdminUserId(id))
            {
                return BadRequest(SD.SuperAdminChangeNotallowed);
            }

            await _userManager.DeleteAsync(user);
            return NoContent();
        }

        [HttpGet]
        [Route("get-application-roles")]
        public async Task<ActionResult<string[]>> GetApplicationRoles()
        {
            return Ok(await _roleManager.Roles.Select(x => x.Name).ToListAsync());
        }

        #region Private Helper Methods
        private bool IsAdminUserId(string userId)
        {
            return _userManager.FindByIdAsync(userId).GetAwaiter().GetResult().UserName.Equals(SD.AdminUserName);
        }
        #endregion


    }
}
