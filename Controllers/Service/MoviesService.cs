using AutoMapper;
using MyMoviesBackend.Models;
using MyMoviesBackend.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMoviesBackend.Controllers.Service
{
    public class MoviesService
    {
        private AppDbContext _context;
        private readonly IMapper _mapper;

        public MoviesService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddMovie(MoviesView movie)
        {
            if (_context.DbUsersMovies.Any(a => a.Title == movie.Title))
            {
                throw new Exception($"Sljedeći film sa nazivom '{movie.Title}' već postoji!");
            }
            var _movie = _mapper.Map<Movies>(movie);
            _movie = new Movies()
            {
                ImdbId = movie.ImdbId,
                Title = movie.Title,
                Plot = movie.Plot,
                Genre = movie.Genre,
                Poster = movie.Poster,
                Released = movie.Released,
                Runtime = movie.Runtime,
                ImdbRating = movie.ImdbRating
            };
            try
            {
                _context.DbMovies.Add(_movie);
                _context.SaveChanges();
            }
            catch
            {
                throw new Exception("Greška u bazi!");
            }
        }

        public List<Movies> GetAllMovies()
        {
            var allMovies = _context.DbMovies.ToList();
            return allMovies;
        }

        public Movies GetMovieById(string Id)
        {
            var GetUser = _context.DbMovies.FirstOrDefault(n => n.ImdbId == Id);
            return GetUser;
        }

        public Movies UpdateMovieById(string Id, MoviesView movieView)
        {
            var _movie = _context.DbMovies.FirstOrDefault(n => n.ImdbId == Id);
            if (_movie != null)
            {
                _movie.Title = movieView.Title;
                _movie.Poster = movieView.Poster;
                _movie.Released = movieView.Released;
                _movie.Runtime = movieView.Runtime;

                _mapper.Map(movieView, _movie);
                _context.SaveChanges();
                return _movie;
            }
            else
            {
                throw new Exception("Greška! Niste unjeli dobar Id filma?");
            }
        }

        public void DeleteMovieById(string id)
        {
            var _movie = _context.DbMovies.FirstOrDefault(n => n.ImdbId == id);
            if (_movie != null)
            {
                _context.DbMovies.Remove(_movie);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Brisanje filma nije uspjelo!");
            }
        }
    }
}
