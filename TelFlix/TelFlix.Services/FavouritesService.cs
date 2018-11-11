using System.Collections.Generic;
using System.Linq;
using TelFlix.Data.Context;
using TelFlix.Data.Models;
using TelFlix.Services.Abstract;
using TelFlix.Services.Contracts;
using TelFlix.Services.Models.Movie;
using TelFlix.Services.Providers.Exceptions;
namespace TelFlix.Services
{
    public class FavouritesService : BaseService, IFavouritesService
    {
        public FavouritesService(TFContext context)
            : base(context)
        {
        }

        //public IEnumerable<ListMovieViewModel> List(User user)
        //{
        //    return this.UnitOfWork
        //        .GetRepo<MoviesUsers>()
        //        .GetAll(x => x.UserId == this.UserManager.GetUserId(user))
        //        .Select(m => new ListMovieViewModel()
        //        {
        //            Title = m.Movie.Title,
        //            Duration = m.Movie.DurationInMinutes,
        //            ReleaseDate = m.Movie.ReleaseDate.Value
        //        })
        //        .ToList();
        //}

        public Movie AddMovieToFavourite(int movieId, string userId)
        {
            Movie movie = this.Context
                .Movies
                .FirstOrDefault(mi => mi.Id == movieId);

            if (movie == null)
            {
                throw new InexistingEntityException(nameof(Movie), movieId.ToString());
            }

            bool isAlreadyThere = this.Context
                .MoviesUsers
                .Any(p => p.UserId == userId && p.MovieId == movie.Id);

            if (isAlreadyThere)
            {
                throw new EntityAlreadyExistingException(nameof(Movie), movie.Title, "favourites !");
            }

            this.Context
                .MoviesUsers
                .Add(new MoviesUsers
                {
                    MovieId = movie.Id,
                    UserId = userId
                });

            this.Context.SaveChanges();

            return movie;
        }

        public bool IsInLibrary(int movieId, string userId)
            => this.Context.MoviesUsers
                .Any(mu => mu.UserId == userId && mu.MovieId == movieId);

        public IEnumerable<ListMovieModel> GetAllFavoritesByUserId(string userId)
            => this.Context
                .MoviesUsers
                .Where(mu => mu.UserId == userId)
                .Select(x => new ListMovieModel
                {
                    Title = x.Movie.Title,
                    Duration = x.Movie.DurationInMinutes,
                    ReleaseDate = x.Movie.ReleaseDate
                })
                .ToList();
    }
}
