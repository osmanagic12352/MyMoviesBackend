using MyMoviesBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMoviesBackend.Controllers.Service
{
    public class FavoriteService
    {
        private AppDbContext _context;

        public FavoriteService(AppDbContext context)
        {
            _context = context;
        }

        public void DeleteFavorite(string id)
        {
            var _fav = _context.DbFavorite.FirstOrDefault(n => n.ImdbId == id);
            if (_fav == null)
            {
                throw new Exception("Neispravan Id!");
            }
            _context.DbFavorite.Remove(_fav);
            _context.SaveChanges();
        }
    }
}
