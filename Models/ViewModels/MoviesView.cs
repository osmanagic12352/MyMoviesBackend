using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMoviesBackend.Models.ViewModels
{
    public class MoviesView
    {
        public string ImdbId { get; set; }
        public string Title { get; set; }
        public string Poster { get; set; }
        public string Released { get; set; }
        public string Runtime { get; set; }
        public string ImdbRating { get; set; }
    }
}
