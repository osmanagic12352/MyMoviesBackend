using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyMoviesBackend.Models
{
    public class Movies
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImdbId { get; set; }
        public string Poster { get; set; }
        public string Plot { get; set; }
        public string Released { get; set; }
        public string Genre { get; set; }
        public string Runtime { get; set; }
        public string ImdbRating { get; set; }

        //Relacija N:N - u više lista, više filmova 
        public List<UsersLists_Movies> UsersLists_Movies { get; set; }

        //Relacija N:N - više filmova, od više usera, u favoritima
        public List<Favorite> Favorites { get; set; }
    }
}
