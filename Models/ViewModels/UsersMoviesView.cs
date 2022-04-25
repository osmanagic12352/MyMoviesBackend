using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyMoviesBackend.Models.ViewModels
{
    public class UsersMoviesView
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Poster { get; set; }
        [Required]
        public string ImdbId { get; set; }
        [Required]
        public string Plot { get; set; }
        [Required]
        public string Released { get; set; }
        [Required]
        public string Runtime { get; set; }
        [Required]
        public string Genre { get; set; }
    }
}
