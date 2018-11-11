using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using TelFlix.Data.Context;
using TelFlix.Data.Models;
using TelFlix.Services;
using TelFlix.Services.Contracts;
using TelFlix.Services.Providers.Exceptions;

namespace TelFlix.Tests.Services
{
    [TestClass]
    public class AddMovieServiceTests
    {
        private const string movieTitle = "The best movie ever!";
        private const int apiMovieIdToUse = 1;
        [TestMethod]
        public void AddMovie_ShouldThrow_WhenMovieIsAlreadyThere()
        {
            var db = new TFContext(this.DatabaseSimulator());
            var genreServicesMock = new Mock<IGenreServices>();
            var actorServicesMock = new Mock<IActorServices>();
            var movieService = new AddMovieService
                (db, genreServicesMock.Object, actorServicesMock.Object);

            Movie movieToAdd = new Movie
            {
                ApiMovieId = apiMovieIdToUse,
                Title = movieTitle

            };

            movieService.AddMovie(movieToAdd);

            Assert.ThrowsException<EntityAlreadyExistingException>(() => movieService.AddMovie(movieToAdd));

        }
        [TestMethod]
        public void AddMovie_Should_RestoreActor()
        {
            var db = new TFContext(this.DatabaseSimulator());
            var genreServicesMock = new Mock<IGenreServices>();
            var actorServicesMock = new Mock<IActorServices>();
            var movieService = new AddMovieService
                (db, genreServicesMock.Object, actorServicesMock.Object);

            Movie movieToAdd = new Movie
            {
                ApiMovieId = apiMovieIdToUse,
                Title = movieTitle
            };

            movieService.AddMovie(movieToAdd);

            movieToAdd.IsDeleted = true;

            movieService.AddMovie(movieToAdd);

            Assert.IsFalse(movieToAdd.IsDeleted);
        }
        [TestMethod]
        public void AddMovie_Should_AddMovieWhenItIsCorrect()
        {
            var db = new TFContext(this.DatabaseSimulator());
            var genreServicesMock = new Mock<IGenreServices>();
            var actorServicesMock = new Mock<IActorServices>();
            var movieService = new AddMovieService
                (db, genreServicesMock.Object, actorServicesMock.Object);

            var movieToAdd = new Movie()
            {
                ApiMovieId = apiMovieIdToUse,
                Title = movieTitle
            };

            var result = movieService.AddMovie(movieToAdd);

            Assert.AreEqual(movieTitle, result.Title);
            Assert.AreEqual(apiMovieIdToUse, result.ApiMovieId);

        }
        [TestMethod]
        public void AddActorToMovie_Should_MakeConnectionsIfActorAlreadyExist()
        {
            var db = new TFContext(this.DatabaseSimulator());
            var genreServicesMock = new Mock<IGenreServices>();
            var actorServicesMock = new Mock<IActorServices>();
            var movieService = new AddMovieService
                (db, genreServicesMock.Object, actorServicesMock.Object);
            
           
            var movie = new Movie()
            {
                Id = 1,
                Title = movieTitle
            };

            var actorToAdd = new Actor()
            {
                Id = 2,
                ApiActorId = 12,
                FullName = "Pesho Peshov"
            };
            db.Actors.Add(actorToAdd);
            db.SaveChanges();
            
            var actorsCast = new List<(Actor, string)>
            {
                (actorToAdd, "a")
            };

            movieService.AddActorsToMovie(movie, actorsCast);

            Assert.AreEqual(1, db.MoviesActors.Count(a => a.ActorId == 2 && a.MovieId == 1));
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
