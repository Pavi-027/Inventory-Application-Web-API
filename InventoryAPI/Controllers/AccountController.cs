using InventoryDTO.DTO.Account;
using InventoryEntities.IdentityEntities;
using InventoryServices.Services;
using InventoryUtility;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JWTService _jwtService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EmailService _emailService;
        private readonly IConfiguration _config;
        protected readonly ILogger<AccountController> _logger;
        public AccountController(JWTService jwtService, 
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            EmailService emailService,
            IConfiguration configuration,
            ILogger<AccountController> logger)
        {
            _jwtService = jwtService;
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = emailService;
            _config = configuration;
            _logger = logger;
        }

        [Authorize]
        [Route("RefreshUserToken")]        
        [HttpGet]
        public async Task<ActionResult<UserDTO>> RefreshUserToken()
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.Email)?.Value);
            return await createApplicationUserDto(user);
        }

        [Route("Login")]
        [HttpPost]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if(user == null)
            {
                return Unauthorized("Invalid username or password");
            }

            if(user.EmailConfirmed == false)
            {
                return Unauthorized("Please confirm your email.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (result.IsLockedOut)
            {
                return Unauthorized(string.Format("Your account has been locked. You should wait unitl {0} (UTC time) to be able to login", user.LockoutEnd));
            }

            if (!result.Succeeded)
            {
                //user has input an invalid password
                if (!user.UserName.Equals(SD.AdminUserName))
                {
                    //Increamenting AccessFailedCount of the AspUser by 1
                    await _userManager.AccessFailedAsync(user);
                }

                if(user.AccessFailedCount >= SD.MaximumLoginAttempts)
                {
                    //lock the user for one day
                    await _userManager.SetLockoutEndDateAsync(user, DateTime.UtcNow.AddDays(1));
                    return Unauthorized(string.Format("Your account has been locked. You should wait unitl {0} (UTC time) to be able to login", user.LockoutEnd));
                }
                return Unauthorized("Invalid username or password");
            }

            //reset the lockout time to 0 when user enter a correct password
            await _userManager.ResetAccessFailedCountAsync(user);
            await _userManager.SetLockoutEndDateAsync(user, null);

            //_logger.LogInformation(1, $"{user.Email} is login successfully");
            return await createApplicationUserDto(user);
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if(await CheckEmailExistsAsync(model.Email))
            {
                return BadRequest($"An existing account is using {model.Email}, email address. Please try with another email address");
            }

            var userToAdd = new ApplicationUser
            {
                FullName = model.FullName,
                UserName = model.Email.ToLower(),
                Email = model.Email.ToLower(),
                StreetAddress = model.StreetAddress,
                City = model.City,
                State = model.State,
                Pincode = model.Pincode,
            };
            userToAdd.CreatedBy = userToAdd.Id;
            userToAdd.UpdatedBy = userToAdd.Id;
               

            var result = await _userManager.CreateAsync(userToAdd, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            try
            {
                if(await SendConfirmEmailAsync(userToAdd))
                {
                    return Ok(new JsonResult(new { title = "Account Created", message = "Your account has been created. Please confirm your email address" }));
                }
                return BadRequest("Failed to send email. Please contact admin");
            }
            catch(Exception)
            {
                return BadRequest("Failed to send email. Please contact admin");
            }
        }

        [HttpPut]
        [Route("confirm-email")]
        public async Task<IActionResult> confirmemail(ConfirmEmailDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user == null)
            {
                return Unauthorized("This email address has not been registered yet.");
            }

            if(user.EmailConfirmed == true)
            {
                return BadRequest("Your email was confirmed before. Please login to your account");
            }

            try
            {
                var decodedTokenBytes = WebEncoders.Base64UrlDecode(model.Token);
                var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);

                var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
                if(result.Succeeded)
                {
                    return Ok(new JsonResult(new { title = "Email Confirmed", message = "Your email address is confirmed. You can login now" }));
                }
                return BadRequest("Invalid token.Please try again");
            }
            catch(Exception)
            {
                return BadRequest("Invalid token.Please try again");
            }
        }

        [HttpPost]
        [Route("resend-email-confirmation-link/{email}")]
        public async Task<IActionResult> ResendEmailConfirmationLink(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Invalid Email");
            }
            var user = await _userManager.FindByEmailAsync(email);

            if(user == null)
            {
                return Unauthorized("this email address has not been registerd yet");
            }

            if(user.EmailConfirmed == true)
            {
                return BadRequest("Your email address was confirmed before. please login to your account");
            }

            try
            {
                if(await SendConfirmEmailAsync(user))
                {
                    return Ok(new JsonResult(new { title = "Confirmation link send", message = "Please confirm your email address" }));
                }
                return BadRequest("Failed to send email. Please contact admin");
            }
            catch(Exception)
            {
                return BadRequest("Failed to send email. Please contact admin");
            }
        }

        [HttpPost]
        [Route(("forgot-username-or-password/{email}"))]
        public async Task<IActionResult> ForgotUsernameOrPassword(string email)
        {
            if(string.IsNullOrEmpty(email))
            {
                return BadRequest("Invalid email");
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Unauthorized("This email address has not been registered yet.");
            }

            if (user.EmailConfirmed == false)
            {
                return BadRequest("Please confirm your email address first.");
            }

            try
            {
                if(await SendForgotUsernameOrPassword(user))
                {
                    return Ok(new JsonResult(new { title = "Forgot username or password email send", message = "Please check your email" }));
                }
                return BadRequest("Failed to send email. Please contact admin");
            }
            catch (Exception)
            {
                return BadRequest("Failed to send email. Please contact admin");
            }
        }

        [HttpPut]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return Unauthorized("This email address has not been registered yet");
            }
            if(user.EmailConfirmed == false)
            {
                return BadRequest("Please confirm your email address first");
            }

            try
            {
                var decodedTokenBytes = WebEncoders.Base64UrlDecode(model.Token);
                var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);

                var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);
                if (result.Succeeded)
                {
                    return Ok(new JsonResult(new { title = "Password reset Success", message = "Your password has been reset" }));
                }
                return BadRequest("Invalid token.Please try again");
            }
            catch(Exception)
            {
                return BadRequest("Invalid token.Please try again");
            }
        }


        #region Private Helper Methods
        private async Task<UserDTO> createApplicationUserDto(ApplicationUser user)
        {
            return new UserDTO
            {
                FullName = user.FullName,
                jwtToken = await _jwtService.CreateJWT(user),
            };
        }

        private async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email.ToLower());
        }

        private async Task<bool> SendConfirmEmailAsync(ApplicationUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var url = $"{_config["JWT:ClientUrl"]}/{_config["Email:ConfirmEmailPath"]}?token={token}&email={user.Email}";

            var body = $"<p>Hello: {user.FullName}</p>" +
                "<p>Please confirm your email address by clicking on the following link.</p>" +
                $"<p><a href=\"{url}\">Click here</a></p>" + 
                "<p>Thank you,</p>" +
                $"<br>{_config["Email:ApplicationName"]}";

            var emailSend = new EmailSendDTO(user.Email, "Confirm your email", body);
            return await _emailService.SendEmailAsync(emailSend);
        }

        private async Task<bool> SendForgotUsernameOrPassword(ApplicationUser user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var url = $"{_config["JWT:ClientUrl"]}/{_config["Email:ResetPasswordPath"]}?token={token}&email={user.Email}";

            var body = $"<p>Hello: {user.FullName}</p>" +
                $"<p>Username: {user.UserName}</p>" +
                "<p>In order to reset your password, please click on the following link.</p>" + 
                $"<p><a href=\"{url}\">Click here</a></p>" +
                "<p>Thank you,</p>" +
                $"<br>{_config["Email:ApplicationName"]}";

            var emailSend = new EmailSendDTO(user.Email, "Forgot username or password", body);
            return await _emailService.SendEmailAsync(emailSend);
        }
        #endregion
    }
}
