using Microsoft.AspNetCore.Mvc.Rendering;

namespace TelFlix.App.Models.Movies
{
    public class MovieIndexViewModel
    {
        // choosen genre id
        public int GenreId { get; set; }
        public SelectList Genres { get; set; }

        public SelectMovieResultViewModel SelectMovieResultViewModel { get; set; }
    }
}
