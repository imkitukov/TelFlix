using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelFlix.Data.Context;
using TelFlix.Data.Models;
using TelFlix.Services.Abstract;
using TelFlix.Services.Contracts;
using TelFlix.Services.Models.Movie;
using TelFlix.Services.Models.Reviews;
using TelFlix.Services.Providers.Exceptions;

namespace TelFlix.Services
{
    public class MovieServices : BaseService, IMovieServices
    {
        private readonly IGenreServices genreServices;

        public MovieServices(TFContext context, IGenreServices genreServices) : base(context)
        {
            this.genreServices = genreServices;
        }

        public IEnumerable<ListMovieModel> GetAllByGenre(int genreId = 0, int page = 1, int pageSize = 3)
        {
            var movies = this.Context
                             .Movies
                             .Where(m => m.IsDeleted == false)
                             .AsQueryable();

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
            var movies = this.Context.Movies.Where(m => m.IsDeleted == false).AsQueryable();

            if (genreId > 0)
            {
                movies = movies.Where(m => m.MoviesGenres.Any(g => g.GenreId == genreId));
            }

            return movies.Count();
        }

        public int Count() => this.Context.Movies.Count(m => m.IsDeleted == false);

        public MovieDetailModel GetMovieById(int id)
        {
            return this.Context
                .Movies
                .Where(m => m.IsDeleted == false)
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
                    Reviews = m.Reviews
                                .OrderByDescending(r => r.CreatedOn)
                                .Select(r => new ReviewModel
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
                 => this.Context
                        .Movies
                        .Where(m => m.IsDeleted == false)
                        .OrderByDescending(m => m.Rating).Take(5)
                        .Select(m => new TopListMovieModel
                        {
                            Id = m.Id,
                            Title = m.Title,
                            DurationInMinutes = m.DurationInMinutes,
                            TrailerUrl = m.TrailerUrl,
                            Rating = m.Rating,
                            SmallImageUrl = m.SmallImageUrl,
                            MediumImageUrl = m.MediumImageUrl
                        });

        public void DeleteById(int id)
        {
            var movie = this.Context.Movies.FirstOrDefault(m => m.Id == id);

            if (movie == null)
            {
                throw new InexistingEntityException(nameof(Movie), id.ToString());
            }

            movie.IsDeleted = true;
            this.Context.SaveChanges();
        }

        public bool Exists(int id) => this.Context.Movies.Any(m => m.IsDeleted == false && m.Id == id);

        public bool ApiIdExists(int apiId)
            => this.Context.Movies.Any(m => m.IsDeleted == false && m.ApiMovieId == apiId);

        public async Task<(int Id, string Title)> GetMovieByApiId(int addedMovieApiId)
        {
            var movie = await this.Context.Movies.FirstOrDefaultAsync(m => m.IsDeleted == false && m.ApiMovieId == addedMovieApiId);

            return (movie.Id, movie.Title);
        }
    }
}
