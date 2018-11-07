using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using TelFlix.Services.Models.Movie;

namespace TelFlix.App.Models.Movies
{
    public class MovieIndexViewModel
    {
        private readonly int totalMoviesInGenre;

        public MovieIndexViewModel(int totalMovies, int pageSize)
        {
            this.totalMoviesInGenre = totalMovies;
            this.TotalPages = (int)Math.Ceiling(this.totalMoviesInGenre / (double)pageSize);
        }

        // choosen genre id
        public int GenreId { get; set; }
        public SelectList Genres { get; set; }

        public IEnumerable<ListMovieModel> Movies { get; set; }

        public int TotalPages { get; private set; }

        public int CurrentPage { get; set; }

        public int PreviousPage => this.CurrentPage == 1
            ? 1
            : this.CurrentPage - 1;

        public int NextPage => this.CurrentPage == this.TotalPages
            ? this.TotalPages
            : this.CurrentPage + 1;
    }
}
