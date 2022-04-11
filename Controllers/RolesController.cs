using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyMoviesBackend.Controllers.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMoviesBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly ILogger<UserController> _logger;
        public RolesService _role;


        public RolesController(
            ILogger<UserController> logger,
            RolesService role,
            RoleManager<IdentityRole<int>> roleManager)
        {
            _logger = logger;
            _role = role;
            _roleManager = roleManager;
        }


        
        [HttpPost]
        [Route("addRole")]
        public async Task<IActionResult> AddRole(string name)
        {
            try
            {
                await _role.AddRole(name);
                var allRoles = _roleManager.Roles.ToList();
                return Ok(allRoles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("getAllRoles")]
        public IActionResult GetAllRoles()
        {
            var allRoles = _role.GetAllRoles();
            return Ok(allRoles);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("getAllUserRoles")]
        public IActionResult GetAllUserRoles()
        {
            var allUserRoles = _role.GetAllUserRoles();
            return Ok(allUserRoles);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("deleteRole_ByName/{name}")]
        public IActionResult DeleteRole(string name)
        {
            try
            {
                _role.DeleteRole(name);
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
