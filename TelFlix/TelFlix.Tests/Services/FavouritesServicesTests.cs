using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using TelFlix.Data.Context;
using TelFlix.Data.Models;
using TelFlix.Services;
using TelFlix.Services.Providers.Exceptions;

namespace TelFlix.Tests.Services
{
    [TestClass]
    public class FavouritesServicesTests
    {
        private const string userIdToUse = "UserId";

        [TestMethod]
        public void AddMovieToFavourite_Should_ThrowWhenMovieDoNotExist()
        {
            var db = new TFContext(this.DatabaseSimulator());
            var favouriteService = new FavouritesService(db);

            Assert.ThrowsException<InexistingEntityException>
                (() => favouriteService.AddMovieToFavourite(1, userIdToUse));
        }
        [TestMethod]
        public void AddMovieToFavourite_Should_ThrowWhenMovieIsAlreadyInFavourite()
        {
            var db = new TFContext(this.DatabaseSimulator());
            var favouriteService = new FavouritesService(db);

            var movie = new Movie()
            {
                Id = 1
            };
            var movieUser = new MoviesUsers()
            {
                MovieId = 1,
                UserId = userIdToUse
            };
            db.Movies.Add(movie);
            db.MoviesUsers.Add(movieUser);
            db.SaveChanges();

            Assert.ThrowsException<EntityAlreadyExistingException>
                (() => favouriteService.AddMovieToFavourite(1, userIdToUse));
        }
        [TestMethod]
        public void AddMovieToFavourite_Should_AddCorrect()
        {
            var db = new TFContext(this.DatabaseSimulator());
            var favouriteService = new FavouritesService(db);

            var movie = new Movie()
            {
                Id = 1
            };
            db.Movies.Add(movie);
            db.SaveChanges();
            favouriteService.AddMovieToFavourite(movie.Id, userIdToUse);
            

            Assert.AreEqual(1, db.MoviesUsers.Count(a => a.UserId == userIdToUse && a.MovieId == 1));
        }
        [TestMethod]
        public void GetAllFavoritesByUserId_Should_ReturnAllFavourites()
        {
            var db = new TFContext(this.DatabaseSimulator());
            var favouriteService = new FavouritesService(db);

            var movie = new Movie()
            {
                Id = 1
            };
            var secondMovie = new Movie()
            {
                Id = 2
            };
            var thridMovie = new Movie()
            {
                Id = 3
            };
            var movieUser = new MoviesUsers()
            {
                UserId = userIdToUse,
                Movie = movie
            };
            var secondMovieUser = new MoviesUsers()
            {
                UserId = userIdToUse,
                Movie = secondMovie
            };
            var thridMovieUser = new MoviesUsers()
            {
                UserId = userIdToUse,
                Movie = thridMovie
            };
            db.Movies.Add(movie);
            db.Movies.Add(secondMovie);
            db.Movies.Add(thridMovie);
            db.MoviesUsers.AddRange(movieUser, secondMovieUser, thridMovieUser);
            db.SaveChanges();


            Assert.AreEqual(3, favouriteService.GetAllFavoritesByUserId(userIdToUse).Count());
        }


        private DbContextOptions<TFContext> DatabaseSimulator()
        {
            var dbOptions = new DbContextOptionsBuilder<TFContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return dbOptions;
        }
    }
}
