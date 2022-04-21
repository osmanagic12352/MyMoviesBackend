using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyMoviesBackend.Models;
using MyMoviesBackend.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyMoviesBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public LoginController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            var UserCheck = await _userManager.FindByNameAsync(login.Email);
            if (UserCheck != null && await _userManager.CheckPasswordAsync(UserCheck, login.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(UserCheck);
                IdentityOptions _options = new IdentityOptions();

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, UserCheck.UserName),
                    new Claim("UserID", UserCheck.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(_options.ClaimsIdentity.RoleClaimType,userRoles.FirstOrDefault())
                };


                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var LoginKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MyMoviesSecretKey123"));

                var TokenSettings = new JwtSecurityToken(
                    issuer: "https://localhost:5002",
                    audience: "https://localhost:5002",
                    expires: DateTime.Now.AddHours(5),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(LoginKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(TokenSettings) });
            }
            else
            {
                throw new Exception("Neuspjela prijava! (Da li ste upisali dobro svoje korisničke podatke?)");
            }
        }
    }
}
