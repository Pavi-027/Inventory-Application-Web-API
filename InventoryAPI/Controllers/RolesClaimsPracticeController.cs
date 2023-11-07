using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesClaimsPracticeController : ControllerBase
    {
        [HttpGet]
        [Route("public")]
        public IActionResult Public()
        {
            return Ok("public");
        }

        #region Roles

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("admin-role")]
        public IActionResult AdminRole()
        {
            return Ok("Admin Role");
        }

        [Authorize(Roles = "Manager")]
        [HttpGet]
        [Route("manager-role")]
        public IActionResult ManagerRole()
        {
            return Ok("Manager Role");
        }

        [Authorize(Roles = "SalesPerson")]
        [HttpGet]
        [Route("salesPerson-role")]
        public IActionResult SalesPersonrole()
        {
            return Ok("SalesPerson Role");
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpGet]
        [Route("admin-or-manager-role")]
        public IActionResult AdminOrManagerRole()
        {
            return Ok("Admin or Manager Role");
        }

        [Authorize(Roles = "Admin, SalesPerson")]
        [HttpGet]
        [Route("admin-or-salesPerson-role")]
        public IActionResult AdminOrSalesPersonRole()
        {
            return Ok("Admin or SalesPerson Role");
        }
        #endregion

        #region Policy
        [HttpGet]
        [Route("admin-policy")]
        [Authorize(policy: "AdminPolicy")]
        public IActionResult AdminPolicy()
        {
            return Ok("Admin Policy");
        }

        [HttpGet]
        [Route("manager-policy")]
        [Authorize(policy: "ManagerPolicy")]
        public IActionResult ManagerPolicy()
        {
            return Ok("Manager Policy");
        }

        [HttpGet]
        [Route("salesPerson-policy")]
        [Authorize(policy: "SalesPersonPolicy")]
        public IActionResult SalesPersonPolicy()
        {
            return Ok("SalesPerson Policy");
        }

        [HttpGet]
        [Route("admin-or-manager-policy")]
        [Authorize(policy: "AdminOrManagerPolicy")]
        public IActionResult AdminOrManagerPolicy()
        {
            return Ok("Admin or Manager Policy");
        }

        [HttpGet]
        [Route("admin-and-manager-policy")]
        [Authorize(policy: "AdminAndManagerPolicy")]
        public IActionResult AdminAndManagerPolicy()
        {
            return Ok("Admin and Manager Policy");
        }

        [HttpGet]
        [Route("allRole-policy")]
        [Authorize(policy: "AllRolePolicy")]
        public IActionResult AllRolePolicy()
        {
            return Ok("All Role Policy");
        }
        #endregion

        #region Claims Policy
        [HttpGet]
        [Route("admin-email-policy")]
        [Authorize(policy: "AdminEmailPolicy")]
        public IActionResult AdminEmailPolicy()
        {
            return Ok("Admin Email Policy");
        }

        [HttpGet]
        [Route("salesPerson-fullname-policy")]
        [Authorize(policy: "salesPersonFullNamePolicy")]
        public IActionResult salesPersonFullNamePolicy()
        {
            return Ok("salesPerson FullName Policy");
        }
        #endregion

    }
}
