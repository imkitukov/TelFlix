//using Microsoft.AspNetCore.Identity;
//using System.Collections.Generic;
//using System.Linq;
//using TelFlix.Data.Models;
//using TelFlix.Data.UnitOfWorkCore;
//using TelFlix.Services.Abstract;
//using TelFlix.Services.Contracts;
//using TelFlix.Services.Providers.Exceptions;
//using TelFlix.Services.ViewModels.MovieViewModels;
//namespace TelFlix.Services
//{
//    public class FavouritesService : BaseService, IFavouritesService
//    {
//        public FavouritesService(IUnitOfWork unitOfWork, UserManager<User> userManager)
//            : base(unitOfWork, userManager)
//        {
//        }

//        public IEnumerable<ListMovieViewModel> List(User user)
//        {
//            return this.UnitOfWork
//                .GetRepo<MoviesUsers>()
//                .GetAll(x => x.UserId == this.UserManager.GetUserId(user))
//                .Select(m => new ListMovieViewModel()
//                {
//                    Title = m.Movie.Title,
//                    Duration = m.Movie.DurationInMinutes,
//                    ReleaseDate = m.Movie.ReleaseDate.Value
//                })
//                .ToList();
//        }

//        public Movie AddMovieToFavourite(string movieTitle, int userId)
//        {
//            Movie movie = this.UnitOfWork
//                  .GetRepo<Movie>()
//                  .GetAll()
//                  .FirstOrDefault(mi => mi.Title == movieTitle);

//            if (movie == null)
//            {
//                throw new InexistingEntityException(nameof(Movie), movieTitle);
//            }

//            bool isAlreadyThere = this.UnitOfWork
//                .GetRepo<MoviesUsers>()
//                .GetAll()
//                .Any(p => p.UserId == userId && p.MovieId == movie.Id);

//            if (isAlreadyThere)
//            {
//                throw new EntityAlreadyExistingException(nameof(Movie), movie.Title, "favourites !");
//            }

//            this.UnitOfWork
//                .GetRepo<MoviesUsers>()
//                .Add(new MoviesUsers
//                {
//                    MovieId = movie.Id,
//                    UserId = userId
//                });

//            this.UnitOfWork.SaveChanges();

//            return movie;
//        }
//    }
//}
