using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMoviesBackend.Models.ViewModels
{
    public class UsersListsMoviesView
    {
        public List<string> ImdbId { get; set; }
        public List<string> Title { get; set; }
        public List<string> Poster { get; set; }
    }
}
