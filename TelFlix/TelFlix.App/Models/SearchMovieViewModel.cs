using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace TelFlix.App.Models
{
    public class SearchMovieViewModel
    {
        public SearchMovieViewModel()
        {
            this.ApiMovies = new List<MovieViewModel>();
            this.DbMovies = new List<MovieViewModel>();
        }

        public string SearchString { get; set; }

        public SelectList Genres;

        public string MovieGenre { get; set; }

        public IEnumerable<MovieViewModel> ApiMovies { get; set; }

        public IEnumerable<MovieViewModel> DbMovies { get; set; }
    }
}
