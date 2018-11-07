using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using TelFlix.Services.Models.Movie;

namespace TelFlix.App.Models.Movies
{
    public class MovieIndexViewModel
    {
        // choosen genre id
        public int GenreId { get; set; }
        public SelectList Genres { get; set; }
       
        public IEnumerable<ListMovieModel> Movies { get; set; }
    }
}
