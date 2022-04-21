using AutoMapper;
using MyMoviesBackend.Models;
using MyMoviesBackend.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMoviesBackend.Controllers.Service
{
    public class UsersListsMoviesService
    {
        private AppDbContext _context;

        public UsersListsMoviesService(AppDbContext context)
        {
            _context = context;
        }

        public void AddMovieInList(int id, string imdbId)
        {
            var _list = _context.DbUsersLists.FirstOrDefault(n => n.Id == id);
            var _imdbId = _context.DbMovies.FirstOrDefault(n => n.ImdbId == imdbId);
            if (_list == null && _imdbId == null)
                throw new Exception("Neispravan unos Id-a filma ili Id liste!");
            var mList = new UsersLists_Movies()
            {
                ListId = id,
                ImdbId = imdbId
            };
            if (_context.DbUsersLists_Movies.Any(x => x.ListId == id && x.ImdbId == imdbId))
                throw new Exception("Film je već dodan u odabranu listu!");
            _context.DbUsersLists_Movies.Add(mList);
            _context.SaveChanges();
        }

        public UsersListsMoviesView GetMovieFromList(int id)
        {
            var _list = _context.DbUsersLists.FirstOrDefault(n => n.Id == id);
            if (_list == null)
                throw new Exception("Niste dobro upisali Id liste!");
            var movie = _context.DbUsersLists.Where(n => n.Id == id).Select(movies => new UsersListsMoviesView()
            {
                ImdbId = movies.UsersLists_Movies.Select(n => n.ImdbId).ToList(),
                Title = movies.UsersLists_Movies.Select(n => n.Movies.Title).ToList(),
                Poster = movies.UsersLists_Movies.Select(n => n.Movies.Poster).ToList()

            }).FirstOrDefault();


            return movie;
        }

        public void DeleteMovieFromList(string id)
        {
            var _mList = _context.DbUsersLists_Movies.FirstOrDefault(n => n.ImdbId == id);
            if (_mList == null)
            {
                throw new Exception("Neispravan Id!");
            }
            _context.DbUsersLists_Movies.Remove(_mList);
            _context.SaveChanges();
        }
    }
}
