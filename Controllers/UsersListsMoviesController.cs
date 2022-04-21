using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class UsersListsMoviesController : ControllerBase
    {
        public UsersListsMoviesService _mList;
        private readonly ILogger<UsersListsMoviesController> _logger;

        public UsersListsMoviesController(UsersListsMoviesService mList, ILogger<UsersListsMoviesController> logger)
        {
            _mList = mList;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("addMovieInList")]
        public IActionResult AddMovieInList(int id, string imdbId)
        {
            try
            {
                _mList.AddMovieInList(id, imdbId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }

        [Authorize]
        [HttpGet("getMovieFromList")]
        public IActionResult GetMovieFromList(int id)
        {
            try
            {
                var result = _mList.GetMovieFromList(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.ToString());
                return BadRequest(ex.ToString());
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("deleteMovieFromList/{id}")]
        public IActionResult DeleteMovieFromList(string id)
        {
            try
            {
                _mList.DeleteMovieFromList(id);
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
