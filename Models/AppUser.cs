using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMoviesBackend.Models
{
    public class AppUser : IdentityUser<int>
    {
        public string FullName { get; set; }


        //Relacija N:N - više filmova, od više usera, u favoritima
        public List<Favorite> Favorites { get; set; }

        //Relacija 1:N - 1 User, više lista
        public List<UsersLists> List { get; set; }

        //Relacija 1:N - 1 User, više filmova
        public List<UsersMovies> UsersMovies { get; set; }
    }
}
