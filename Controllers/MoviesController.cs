using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    public class MoviesController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private AppDbContext _context;
        private readonly IMapper _mapper;
        public MoviesService _movie;
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(
            UserManager<AppUser> userManager,
            AppDbContext context,
            MoviesService movies,
            IMapper mapper,
            ILogger<MoviesController> logger)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
            _movie = movies;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("addMovie")]
        public IActionResult AddMovie(MoviesView movie)
        {
            try 
            {
                _movie.AddMovie(movie);
                return Ok("Success");
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
        public IActionResult GetAllMovies()
        {
            var allmovies = _movie.GetAllMovies();
            return Ok(allmovies);
        }


        [Authorize]
        [HttpPut]
        [Route("editMovie_ById/{id}")]
        public IActionResult UpdateMovieById(int id, [FromBody] MoviesView movie)
        {
            try
            {
                var MovieUpdate = _movie.UpdateMovieById(id, movie);
                return Ok(MovieUpdate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }

        }

        [Authorize]
        [HttpDelete]
        [Route("deleteMovie_ById/{id}")]
        public IActionResult DeleteMovieById(int id)
        {
            try
            {
                _movie.DeleteMovieById(id);
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
