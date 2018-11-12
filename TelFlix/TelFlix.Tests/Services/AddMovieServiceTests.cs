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
        private const int movieIdToUse = 1;
        private const int actorIdToUse = 2;
        private const int apiActorIdToUse = 12;
        private const string actorName = "Pesho Peshov";
        private const string actorRole = "Captain Pesho Sparrow";


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
                Id = movieIdToUse,
                Title = movieTitle
            };

            var actorToAdd = new Actor()
            {
                Id = actorIdToUse,
                ApiActorId = apiActorIdToUse,
                FullName = actorName
            };
            db.Actors.Add(actorToAdd);
            db.SaveChanges();
            
            var actorsCast = new List<(Actor, string)>
            {
                (actorToAdd, actorRole)
            };

            movieService.AddActorsToMovie(movie, actorsCast);

            Assert.AreEqual(1, db.MoviesActors.Count(a => a.ActorId == 2 && a.MovieId == 1));
        }
        //[TestMethod]
        //public void AddActorToMovie_Should_MakeConnectionsIfActorDoNotExist()
        //{
        //    var db = new TFContext(this.DatabaseSimulator());
        //    var genreServicesMock = new Mock<IGenreServices>();
        //    var actorServicesMock = new Mock<IActorServices>();
        //    var movieService = new AddMovieService
        //        (db, genreServicesMock.Object, actorServicesMock.Object);


        //    var movie = new Movie()
        //    {
        //        Id = movieIdToUse,
        //        Title = movieTitle
        //    };

        //    var actorToAdd = new Actor()
        //    {
        //        Id = actorIdToUse,
        //        ApiActorId = apiActorIdToUse,
        //        FullName = actorName
        //    };

        //    var actorsCast = new List<(Actor, string)>
        //    {
        //        (actorToAdd, actorRole)
        //    };

        //    actorServicesMock.Object.AddActor(actorToAdd);

        //    movieService.AddActorsToMovie(movie, actorsCast);

        //    Assert.AreEqual(1, db.MoviesActors.Count(a => a.ActorId == 2 && a.MovieId == 1));
        //}
        [TestMethod]
        public void AddGenreToMovie_Should_MakeConnectionsIfGenreAlreadyExist()
        {
            var db = new TFContext(this.DatabaseSimulator());
            var genreServicesMock = new Mock<IGenreServices>();
            var actorServicesMock = new Mock<IActorServices>();
            var movieService = new AddMovieService
                (db, genreServicesMock.Object, actorServicesMock.Object);

            var movie = new Movie()
            {
                Id = movieIdToUse,
                Title = movieTitle
            };

            var genreToAdd = new Genre()
            {
                Id = 2,
                Name = "Action",
                ApiGenreId = 2
                
            };
            db.Genres.Add(genreToAdd);
            db.SaveChanges();

            var genresCast= new List<(int, string)>
            {
                (genreToAdd.Id, genreToAdd.Name)
            };

            movieService.AddGenresToMovie(movie, genresCast);

            Assert.AreEqual(1, db.MoviesGenres.Count(a => a.GenreId == 2 && a.MovieId == 1));


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
