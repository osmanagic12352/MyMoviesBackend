using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class UsersListsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private AppDbContext _context;
        private readonly IMapper _mapper;
        public UsersListsService _lists;
        private readonly ILogger<UsersListsController> _logger;

        public UsersListsController(
            UserManager<AppUser> userManager,
            AppDbContext context,
            UsersListsService lists,
            IMapper mapper,
            ILogger<UsersListsController> logger)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
            _lists = lists;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("addUsersList")]
        public async Task<IActionResult> AddUsersList(UsersListsView list)
        {
            var userId = User.Claims.First(a => a.Type == "UserID").Value;
            var _user = await _userManager.FindByIdAsync(userId);
            if (_user != null)
            {
               
                var _list = _mapper.Map<UsersLists>(list);
                _list = new UsersLists()
                {
                    ListName = list.ListName,
                    UserId = int.Parse(userId)
                };
                _context.DbUsersLists.Add(_list);
                _context.SaveChanges();
                return Ok("Success");
            }
            else
            {
                throw new Exception("Pravljenje liste nije uspjelo!");
            }
        }

        [Authorize]
        [HttpGet("getUsersLists_User")]
        public async Task<IActionResult> GetUsersLists()
        {
            var userId = User.Claims.First(a => a.Type == "UserID").Value;
            var usersL = await _context.DbUsersLists.Where(n => n.UserId == Int32.Parse(userId)).ToListAsync();
            return Ok(usersL);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("getAllUsersLists")]
        public IActionResult GetAllUsersLists()
        {
            var allLists = _lists.GetAllUsersLists();
            return Ok(allLists);
        }


        [Authorize]
        [HttpPut]
        [Route("editUsersList_ById/{id}")]
        public IActionResult UpdateUsersListsById(int id, [FromBody] UsersListsView list)
        {
            try
            {
                var ListUpdate = _lists.UpdateUsersListsById(id, list);
                return Ok(ListUpdate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }

        }

        [Authorize]
        [HttpDelete]
        [Route("deleteUsersList_ById/{id}")]
        public IActionResult DeleteUsersListsById(int id)
        {
            try
            {
                _lists.DeleteUsersListsById(id);
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
