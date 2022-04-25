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
        public MoviesService _movie;
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(
            MoviesService movies,
            ILogger<MoviesController> logger)
        {
            _movie = movies;
            _logger = logger;
        }


        [HttpPost]
        [Route("addMovie")]
        public IActionResult AddMovie(MoviesView movie)
        {
            try 
            {
                _movie.AddMovie(movie);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet]
        [Route("getAllMovies")]
        public IActionResult GetAllMovies()
        {
            var allmovies = _movie.GetAllMovies();
            return Ok(allmovies);
        }

        [HttpGet]
        [Route("getMovie_ById/{id}")]
        public IActionResult GetMovieById(string id)
        {
            var GetUser = _movie.GetMovieById(id);
            return Ok(GetUser);
        }


        [Authorize]
        [HttpPut]
        [Route("editMovie_ById/{id}")]
        public IActionResult UpdateMovieById(string id, [FromBody] MoviesView movie)
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
        public IActionResult DeleteMovieById(string id)
        {
            try
            {
                _movie.DeleteMovieById(id);
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
