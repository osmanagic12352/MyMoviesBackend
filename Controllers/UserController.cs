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

        private readonly ILogger<UserController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private IPasswordHasher<AppUser> _passwordHasher;
        public UserService _user;


        public UserController(
            UserManager<AppUser> userManager, 
            ILogger<UserController> logger, 
            IPasswordHasher<AppUser> passwordHash, 
            UserService user,
            RoleManager<IdentityRole<int>> roleManager)
        {
            _logger = logger;
            _passwordHasher = passwordHash;
            _user = user;
            _userManager = userManager;
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

        [Authorize]
        [HttpGet("getLogedUser")]
        public async Task<Object> GetLogedUser()
        {
            string userId = User.Claims.First(a => a.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            return new
            {
                user.Id,
                user.Email,
                user.UserName,
                user.FullName
            };
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
