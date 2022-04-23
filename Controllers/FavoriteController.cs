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
    public class FavoriteController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;
        private AppDbContext _context;
        public FavoriteService _favorite;

        public FavoriteController(
            UserManager<AppUser> userManager,
            ILogger<UserController> logger,
            IMapper mapper,
            AppDbContext context,
            FavoriteService favorite)
        {
            _logger = logger;
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
            _favorite = favorite;
        }

        [Authorize]
        [HttpPost("addFavorite")]
        public async Task<IActionResult> AddFavorite(string id)
        {
            var userId = User.Claims.First(a => a.Type == "UserID").Value;
            var _user = await _userManager.FindByIdAsync(userId);
            if (_user != null)
            {
                var _movie = _context.DbMovies.FirstOrDefault(n => n.ImdbId == id);
                if (_movie != null)
                {
                    var _fav = new Favorite()
                    {
                        ImdbId = id,
                        UserId = int.Parse(userId)
                    };
                    if (_context.DbFavorite.Any(a => a.UserId == Int32.Parse(userId) && a.ImdbId == id))
                        throw new Exception("Film je već dodan u favorite!");
                    _context.DbFavorite.Add(_fav);
                    _context.SaveChanges();
                    
                }
                else
                {
                    throw new Exception("ImdbId filma nije točan ili ne postoji u bazi!");
                }
                return Ok();
            }
            else
            {
                throw new Exception("Dodavanje u favorite nije uspjelo!");
            }
        }

        [Authorize]
        [HttpGet("getUsersFavorite")]
        public async Task<IActionResult> GetUsersFavoriteMovies()
        {
            string userId = User.Claims.First(a => a.Type == "UserID").Value;
            var user = await _context.DbUsers.Where(n => n.Id == Int32.Parse(userId)).Select(movies => new FavoriteView()

            {
                ImdbId = movies.Favorites.Select(n => n.ImdbId).ToArray().ToList(),
                Title = movies.Favorites.Select(n => n.Movies.Title).ToArray().ToList(),
                Poster = movies.Favorites.Select(n => n.Movies.Poster).ToArray().ToList()

            }).FirstOrDefaultAsync();


            return Ok(user);
        }

        [Authorize]
        [HttpDelete]
        [Route("deleteFavorite/{id}")]
        public IActionResult DeleteFavorite(string id)
        {
            try
            {
                _favorite.DeleteFavorite(id);
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
