using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyMoviesBackend.Models
{
    public class UsersLists
    {
        [Key]
        public int Id { get; set; }
        public string ListName { get; set; }

        //Relacija 1:N - 1 User, više lista        
        public AppUser User { get; set; }
        public int UserId { get; set; }

        //Relacija N:N - u više lista, više filmova 
        public List<UsersLists_Movies> UsersLists_Movies { get; set; }
    }
}
