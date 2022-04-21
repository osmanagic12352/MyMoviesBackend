using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyMoviesBackend.Models
{
    public class UsersLists_Movies
    {
        [Key]
        public int Id { get; set; }

        //Relacija N:N - u više lista, više filmova    
        public UsersLists List { get; set; }
        public int ListId { get; set; }  
        public Movies Movies { get; set; }
        public string ImdbId { get; set; }
    }
}
