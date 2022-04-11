using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MyMoviesBackend.Controllers.Service;
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
    public class UserController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly ILogger<UserController> _logger;
        private IPasswordHasher<AppUser> _passwordHasher;
        public UserService _user;


        public UserController(
            UserManager<AppUser> userManager, 
            ILogger<UserController> logger, 
            IPasswordHasher<AppUser> passwordHash, 
            UserService user,
            RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _logger = logger;
            _passwordHasher = passwordHash;
            _user = user;
            _roleManager = roleManager;
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

        [HttpPost]
        [Route("registerUser")]
        public async Task<IActionResult> InsertUser([FromBody] AppUserView userView)
        {
            try
            {
                await _user.InsertUser(userView);
                return Ok(userView);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }

        }

        [HttpPost]
        [Route("registerAdmin")]
        public async Task<IActionResult> InsertAdmin([FromBody] AppUserView adminView)
        {
            try
            {
                await _user.InsertAdmin(adminView);
                return Ok(adminView);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("getAllUsers")]
        public IActionResult GetAllUsers()
        {
            var allUsers = _user.GetAllUsers();
            return Ok(allUsers);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("getUser_ById/{id}")]
        public IActionResult GetUserById(int id)
        {
            var GetUser = _user.GetUserById(id);
            return Ok(GetUser);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("editUser_ById/{id}")]
        public IActionResult UpdateUserById(int id, [FromBody] AppUserView user)
        {
            try
            {
                var UserUpdate = _user.UpdateUserById(id, user);
                return Ok(UserUpdate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("deleteUser_ById/{id}")]
        public IActionResult DeleteUserById(int id)
        {
            try
            {
                _user.DeleteUserById(id);
                return Ok("Success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }
    }
}
