using InventoryEntities.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.SqlServer.Server;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InventoryServices.Services
{
    public class JWTService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _jwtKey;
        private readonly UserManager<ApplicationUser> _userManager;
        public JWTService(IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            _config = config;
            _userManager = userManager;

            //jwtkey is used for both encripting and decripting the jwt token
            _jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
        }
        public async Task<string> CreateJWT(ApplicationUser user)
        {
            if (user == null || _config == null)
            {
                // Handle null values or configuration issues.
                return null;
            }

            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id ?? string.Empty),
                new Claim(ClaimTypes.Email, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.GivenName, user.FullName ?? string.Empty)
            };

            var roles = await _userManager.GetRolesAsync(user);
            userClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var creadentials = new SigningCredentials(_jwtKey, SecurityAlgorithms.HmacSha256Signature);

            // Get the expiration days from the configuration or use a default value if not set or invalid.
            int expiresInDays;
            if (!int.TryParse(_config["JWT:ExpiresInDays"], out expiresInDays) || expiresInDays <= 0)
            {
                expiresInDays = 7; // Default to 7 days if the configuration value is invalid or missing.
            }


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(userClaims),
                Expires = DateTime.UtcNow.AddDays(expiresInDays),
                //Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = creadentials,
                Issuer = _config["JWT:Issuer"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(jwt);
        }
    }
}
