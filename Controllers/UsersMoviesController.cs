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
    public class UsersMoviesController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private AppDbContext _context;
        private readonly IMapper _mapper;
        public UsersMoviesService _movie;
        private readonly ILogger<UsersMoviesController> _logger;

        public UsersMoviesController( 
            UserManager<AppUser> userManager, 
            AppDbContext context,
            UsersMoviesService movies,
            IMapper mapper,
            ILogger<UsersMoviesController> logger)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
            _movie = movies;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("addUsersMovie")]
        public async Task<IActionResult> AddUsersMovie(UsersMoviesView movie)
        {
            var userId = User.Claims.First(a => a.Type == "UserID").Value;
            var _user = await _userManager.FindByIdAsync(userId);
            if (_user != null)
            {
                if (_context.DbUsersMovies.Any(a => a.Title == movie.Title))
                    throw new Exception($"Sljedeći film sa nazivom '{movie.Title}' već postoji!");
                var _movie = _mapper.Map<UsersMovies>(movie);
                _movie = new UsersMovies()
                {
                    Title = movie.Title,
                    ImdbId = movie.ImdbId,
                    Poster = movie.Poster,
                    Plot = movie.Plot,
                    Genre = movie.Genre,
                    Released = movie.Released,
                    Runtime = movie.Runtime,
                    UserId = int.Parse(userId)
                };
                _context.DbUsersMovies.Add(_movie);
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                throw new Exception("Unjeli ste nepostojeći Id korisnika ili se isti već koristi!");
            }
        }

        [Authorize]
        [HttpGet("getUsersMovies_User")]
        public async Task<IActionResult> GetUserCard()
        {
            var userId = User.Claims.First(a => a.Type == "UserID").Value;
            var usersM = await _context.DbUsersMovies.Where(n => n.UserId == Int32.Parse(userId)).ToListAsync();
            return Ok(usersM);
        }

        [HttpGet]
        [Route("getAllUsersMovies")]
        public IActionResult GetAllUsersMovies()
        {
            var allmovies = _movie.GetAllUsersMovies();
            return Ok(allmovies);
        }


        [Authorize]
        [HttpPut]
        [Route("editUsersMovie_ById/{id}")]
        public IActionResult UpdateUserMovieById(int id, [FromBody] UsersMoviesView movie)
        {
            try
            {
                var UserUpdate = _movie.UpdateUsersMovieById(id, movie);
                return Ok(UserUpdate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }

        }

        [Authorize]
        [HttpDelete]
        [Route("deleteUsersMovie_ById/{id}")]
        public IActionResult DeleteUsersMovieById(int id)
        {
            try
            {
                _movie.DeleteUsersMovieById(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }
    }
}
