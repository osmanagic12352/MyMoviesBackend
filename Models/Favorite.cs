using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyMoviesBackend.Models
{
    public class Favorite
    {
        [Key]
        public int Id { get; set; }

        //Relacija N:N - više filmova, od više usera, u favoritima        
        public Movies Movies { get; set; }
        public string ImdbId { get; set; }      
        public AppUser User { get; set; }
        public int UserId { get; set; }
    }
}
