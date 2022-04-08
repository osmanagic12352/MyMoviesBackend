using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyMoviesBackend.Controllers.Service;
using MyMoviesBackend.Models;
using MyMoviesBackend.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMoviesBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<UserController> _logger;
        private IPasswordHasher<AppUser> _passwordHasher;
        public UserService _user;


        public UserController(UserManager<AppUser> userManager, ILogger<UserController> logger, IPasswordHasher<AppUser> passwordHash, UserService user)
        {
            _userManager = userManager;
            _logger = logger;
            _passwordHasher = passwordHash;
            _user = user;
        }


        [HttpPost("userRegistration")]
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
        [Route("adminRegistration")]
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

        [HttpGet("getAllUsers")]
        public IActionResult GetAllUsers()
        {
            var allUsers = _user.GetAllUsers();
            return Ok(allUsers);
        }


    }
}
