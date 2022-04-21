using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyMoviesBackend.Models;
using MyMoviesBackend.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMoviesBackend.Controllers.Service
{
    public class UsersMoviesService
    {
        private AppDbContext _context;
        private readonly IMapper _mapper;

        public UsersMoviesService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<UsersMovies> GetAllUsersMovies()
        {
            var allmovies = _context.DbUsersMovies.ToList();
            return allmovies;
        }

        public UsersMovies UpdateUsersMovieById(int Id, UsersMoviesView movieView)
        {
            var _movie = _context.DbUsersMovies.FirstOrDefault(n => n.Id == Id);
            if (_movie != null)
            {
                _movie.Title = movieView.Title;
                _movie.Poster = movieView.Poster;
                _movie.Plot = movieView.Plot;
                _movie.Released = movieView.Released;
                _movie.Runtime = movieView.Runtime;
                _movie.Genre = movieView.Genre;

                _mapper.Map(movieView, _movie);
                _context.SaveChanges();
                return _movie;
            }
            else
            {
                throw new Exception("Greška! Niste unjeli dobar Id filma?");
            }
        }

        public void DeleteUsersMovieById(int id)
        {
            var _movie = _context.DbUsersMovies.FirstOrDefault(n => n.Id == id);
            if (_movie != null)
            {
                _context.DbUsersMovies.Remove(_movie);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Brisanje filma nije uspjelo!");
            }
        }
    }
}
