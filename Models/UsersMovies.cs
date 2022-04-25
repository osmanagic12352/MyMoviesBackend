using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyMoviesBackend.Models
{
    public class UsersMovies
    {
        [Key]
        public int Id { get; set; }
        public string ImdbId { get; set; }
        public string Title { get; set; }
        public string Poster { get; set; }
        public string Plot { get; set; }
        public string Released { get; set; }
        public string Runtime { get; set; }
        public string Genre { get; set; }

        //Relacija 1:N - 1 User, više UsersMovies        
        public AppUser User { get; set; }
        public int UserId { get; set; }
    }
}
