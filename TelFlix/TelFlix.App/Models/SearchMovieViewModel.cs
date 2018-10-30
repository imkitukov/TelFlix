using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace TelFlix.App.Models
{
    public class SearchMovieViewModel
    {
        public SearchMovieViewModel()
        {
            this.Movies = new List<MovieViewModel>();
        }

        public string SearchString { get; set; }

        public SelectList Genres;

        public string MovieGenre { get; set; }

        public IEnumerable<MovieViewModel> Movies { get; set; }
    }
}
