using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TelFlix.Data.Context;
using TelFlix.Data.Models;
using TelFlix.Services.Abstract;
using TelFlix.Services.Contracts;
using TelFlix.Services.Models.Movie;
using TelFlix.Services.Models.Reviews;

namespace TelFlix.Services
{
    public class MovieServices : BaseService, IMovieServices
    {
        private readonly IGenreServices genreServices;

        public MovieServices(TFContext context, IGenreServices genreServices) : base(context)
        {
            this.genreServices = genreServices;
        }

        public IEnumerable<ListMovieModel> ListAllMovies(int genreId = 0, int page = 1, int pageSize = 3)
        {
            var movies = this.Context
                             .Movies.AsQueryable();

            if (genreId > 0)
            {
                movies = movies.Where(m => m.MoviesGenres.Any(g => g.GenreId == genreId));
            }

            return movies
                      .OrderByDescending(m => m.Id)
                      .Skip((page - 1) * pageSize)
                      .Take(pageSize)
                      .Select(m => new ListMovieModel
                      {
                          Id = m.Id,
                          ApiMovieId = m.ApiMovieId,
                          Title = m.Title,
                          Duration = m.DurationInMinutes,
                          Rating = m.Rating,
                          ReleaseDate = m.ReleaseDate,
                          Genres = m.MoviesGenres.Select(mg => mg.Genre).ToList()
                      })
                   .ToList();
        }

        public int TotalMoviesInGenre(int genreId)
        {
            var movies = this.Context.Movies.AsQueryable();

            if (genreId > 0)
            {
                movies = movies.Where(m => m.MoviesGenres.Any(g => g.GenreId == genreId));
            }

            return movies.Count();
        }


        public int Count() => this.Context.Movies.Count();

        public MovieDetailModel GetMovieById(int id)
        {
            return this.Context
                .Movies
                    .Include(m => m.Reviews)
                        .ThenInclude(r => r.Author)
                .Select(m => new MovieDetailModel
                {
                    Id = m.Id,
                    ApiMovieId = m.ApiMovieId,
                    Actors = m.MoviesActors.Select(a => a.Actor).ToList(),
                    Genres = m.MoviesGenres.Where(g => g.IsDeleted == false).Select(g => g.Genre).ToList(),
                    Description = m.Description,
                    DurationInMinutes = m.DurationInMinutes,
                    MediumImageUrl = m.MediumImageUrl,
                    Rating = m.Rating,
                    ReleaseDate = m.ReleaseDate,
                    Reviews = m.Reviews.Select(r => new ReviewModel
                    {
                        Id = r.Id,
                        Author = r.Author.Email,
                        CreatedOn = r.CreatedOn,
                        Comment = r.Comment
                    }).ToList(),
                    SmallImageUrl = m.SmallImageUrl,
                    Title = m.Title,
                    TrailerUrl = m.TrailerUrl
                })
                .FirstOrDefault(m => m.Id == id);
        }

        public IEnumerable<Movie> ListTopTenMovies()
        {
            var result = this.Context.Movies
                              .OrderByDescending(r => r.Rating)
                              .Take(10)
                              .ToList();

            return result;
        }

        public IEnumerable<Movie> SearchMovie(string searchString)
        {
            var movies = this.Context
                .Movies
                .Where(m => m.Title.ToLower().Contains(searchString.ToLower()));

            return movies;
        }

        public void Edit(
            int movieId,
            string title,
            string description,
            int? durationInMinutes,
            string trailerUrl,
            IEnumerable<int> selectedGenresIds,
            IEnumerable<int> genresIdsToRemove)
        {
            var movie = this.Context.Movies.Find(movieId);

            if (movie == null)
            {
                return;
            }

            movie.Title = title;
            movie.Description = description;
            movie.DurationInMinutes = durationInMinutes;
            movie.TrailerUrl = trailerUrl;

            genreServices.UpdateMovieGenres(movieId, selectedGenresIds, genresIdsToRemove);

            this.Context.SaveChanges();
        }

        public IEnumerable<TopListMovieModel> GetTop5ByRating()
        {
            throw new System.NotImplementedException();
        }
    }
}
