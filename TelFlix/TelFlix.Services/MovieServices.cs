using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TelFlix.Data.Context;
using TelFlix.Data.Models;
using TelFlix.Services.Abstract;
using TelFlix.Services.Contracts;
using TelFlix.Services.Models.Movie;

namespace TelFlix.Services
{
    public class MovieServices : BaseService, IMovieServices
    {
        public MovieServices(TFContext context) : base(context)
        {
        }

        public IEnumerable<ListMovieModel> ListAllMovies()
        {
            return this.Context
                       .Movies
                       //.Include(m => m.MoviesGenres)
                       //    .ThenInclude(lmg => lmg.Select(mg => mg.Genre))
                       .Select(m => new ListMovieModel
                       {
                           ApiMovieId= m.ApiMovieId,
                           Title = m.Title,
                           Duration = m.DurationInMinutes,
                           Rating = m.Rating,
                           ReleaseDate = m.ReleaseDate,
                           Genres = m.MoviesGenres.Select(mg => mg.Genre).ToList()                           
                       })
                       .ToList();
        }

        public Movie GetMovieById(int id) => this.Context.Movies.Find(id);

        //public IEnumerable<ListMovieModel> ListMoviesInRange(int firstYear, int secondYear)
        //{
        //    var result = this.movieRepo
        //        .GetAll()
        //        .Select(m => new ListMovieViewModel
        //        {
        //            Title = m.Title,
        //            ReleaseDate = m.ReleaseDate.Value,
        //            Duration = m.DurationInMinutes
        //        })
        //        .Where(dt => dt.ReleaseDate.Year >= firstYear && dt.ReleaseDate.Year <= secondYear)
        //        .OrderBy(or => or.ReleaseDate)
        //        .ToList();

        //    return result;
        //}

        public IEnumerable<Movie> ListTopTenMovies()
        {
            var result = this.Context.Movies
                              .OrderByDescending(r => r.Rating)
                              .Take(10)
                              .ToList();

            return result;
        }

        //public IEnumerable<ListGenresViewModel> ListGenres()
        //{
        //    var result = this.movieRepo
        //        .GetAll()
        //        .Select(g => new ListGenresViewModel { Id = g.Id, Genres = g.Name })
        //        .ToList();

        //    return result;
        //}

        public IEnumerable<Movie> SearchMovie(string searchString)
        {
            var movies = this.Context
                .Movies
                .Where(m => m.Title.ToLower().Contains(searchString.ToLower()));

            return movies;
        }

        //public IEnumerable<ListMoviesByGenreViewModel> ListMoviesByGenre(string genreName)
        //{
        //    var result = this.movieRepo
        //        .GetAll()
        //        .Where(mg => mg.Genre.Name == genreName)
        //        .Select(p => new ListMoviesByGenreViewModel
        //        {
        //            Title = p.Movie.Title,
        //            GenreName = p.Genre.Name
        //        })
        //        .ToList();

        //    return result;
        //}
    }
}
