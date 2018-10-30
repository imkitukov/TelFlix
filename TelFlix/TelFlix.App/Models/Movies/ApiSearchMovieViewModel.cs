using System.Collections.Generic;

namespace TelFlix.App.Models
{
    public class ApiSearchMovieViewModel
    {
        public string SearchString { get; set; }

        public IEnumerable<MovieViewModel> FoundMovies { get; set; } = new List<MovieViewModel>();
    }
}
