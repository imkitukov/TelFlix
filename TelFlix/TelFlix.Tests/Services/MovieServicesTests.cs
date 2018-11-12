
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using TelFlix.Data.Context;
using TelFlix.Data.Models;
using TelFlix.Services;
using TelFlix.Services.Contracts;
using TelFlix.Services.Providers.Exceptions;

namespace TelFlix.Tests.Services
{
    [TestClass]
    public class MovieServicesTests
    {
        [TestMethod]
        public void GetAllByGenre_Should_GetThemCorrect()
        {
            var db = new TFContext(DatabaseSimulator());
            var genreServiceMock = new Mock<IGenreServices>();
            var movieServices = new MovieServices(db, genreServiceMock.Object);

            var movie = new Movie()
            {
                Id = 1
            };
            var secondMovie = new Movie()
            {
                Id = 2
            };
            var deletedMovie = new Movie()
            {
                Id = 3,
                IsDeleted = true
            };
            var differentGenreMovie = new Movie()
            {
                Id = 4
            };
            var movieGenre = new MoviesGenres()
            {
                MovieId = 1,
                GenreId = 1
            };
            var secondMovieGenre = new MoviesGenres()
            {
                MovieId = 2,
                GenreId = 1
            };
            var thirdMovieGenre = new MoviesGenres()
            {
                MovieId = 3,
                GenreId = 1
            };
            var differentMovieGenre = new MoviesGenres()
            {
                MovieId = 4,
                GenreId = 2
            };
            db.Movies.AddRange(movie, secondMovie, deletedMovie, differentGenreMovie);
            db.MoviesGenres.AddRange(movieGenre, secondMovieGenre, thirdMovieGenre, differentMovieGenre);
            db.SaveChanges();

            Assert.AreEqual(2, movieServices.GetAllByGenre(1).Count());
        }
        [TestMethod]
        public void TotalMoviesInGenre_Should_CountAllMoviesInGenreCorrect()
        {
            var db = new TFContext(DatabaseSimulator());
            var genreServiceMock = new Mock<IGenreServices>();
            var movieServices = new MovieServices(db, genreServiceMock.Object);
            var movie = new Movie()
            {
                Id = 1
            };
            var secondMovie = new Movie()
            {
                Id = 2
            };
            var thirdMovie = new Movie()
            {
                Id = 3,
            };
            var movieGenre = new MoviesGenres()
            {
                MovieId = 1,
                GenreId = 1
            };
            var secondMovieGenre = new MoviesGenres()
            {
                MovieId = 2,
                GenreId = 1
            };
            var thirdMovieGenre = new MoviesGenres()
            {
                MovieId = 3,
                GenreId = 1
            };
            db.Movies.Add(movie);
            db.Movies.Add(secondMovie);
            db.Movies.Add(thirdMovie);
            db.MoviesGenres.AddRange(movieGenre, secondMovieGenre, thirdMovieGenre);
            db.SaveChanges();

            Assert.AreEqual(3, movieServices.TotalMoviesInGenre(1));
        }
        [TestMethod]
        public void GetMovieById_Should_GetCorrectMovie()
        {
            var db = new TFContext(DatabaseSimulator());
            var genreServiceMock = new Mock<IGenreServices>();
            var movieService = new MovieServices(db, genreServiceMock.Object);

            var movie = new Movie()
            {
                Id = 1,
                Title = "TheMovie"
            };

            db.Movies.Add(movie);
            db.SaveChanges();

            string movieTitle = movieService.GetMovieById(1).Title;

            Assert.AreEqual("TheMovie", movieTitle);
        }
        [TestMethod]
        public void SearchMovie_Should_FindTheCorrectOnes()
        {
            var db = new TFContext(DatabaseSimulator());
            var genreServiceMock = new Mock<IGenreServices>();
            var movieService = new MovieServices(db, genreServiceMock.Object);

            var movie = new Movie()
            {
                Id = 1,
                Title = "The Movie"
            };
            var secondMovie = new Movie()
            {
                Id = 2,
                Title = "The Movies"
            };
            var movieWithoutThe = new Movie()
            {
                Id = 3,
                Title = "Movie"
            };
            db.Movies.AddRange(movie, secondMovie, movieWithoutThe);
            db.SaveChanges();

            int movieCount = movieService.SearchMovie("The").Count();

            Assert.AreEqual(2, movieCount);
        }
        [TestMethod]
        public void GetTop5ByRating_Should_WorkCorrect()
        {
            var db = new TFContext(DatabaseSimulator());
            var genreServiceMock = new Mock<IGenreServices>();
            var movieService = new MovieServices(db, genreServiceMock.Object);

            var movie = new Movie()
            {
                Id = 1,
                Title = "The Movie",
                Rating = 3.5f

            };
            var secondMovie = new Movie()
            {
                Id = 2,
                Title = "The Movies",
                Rating = 5
            };
            var thirdMovie = new Movie()
            {
                Id = 3,
                Title = "Movie",
                Rating = 8
            };
            var fourthMovie = new Movie()
            {
                Id = 4,
                Title = "Movie",
                Rating = 3
            };
            var fifth = new Movie()
            {
                Id = 5,
                Title = "Movie",
                Rating = 1
            };
            var sixth = new Movie()
            {
                Id = 6,
                Title = "Movie",
                Rating = 2
            };
            db.Movies.AddRange
                (movie, secondMovie, thirdMovie, fourthMovie, fifth, sixth);
            db.SaveChanges();

            float? ratingOfTheFirstOne = movieService.GetTop5ByRating().First().Rating;
            float? ratingOfTheLastOne = movieService.GetTop5ByRating().Last().Rating;


            Assert.AreEqual(5, movieService.GetTop5ByRating().Count());
            Assert.AreEqual(8, ratingOfTheFirstOne);
            Assert.AreEqual(2, ratingOfTheLastOne);
        }
        [TestMethod]
        public void DeleteById_Should_ThrowWhenMovieDoesNotExist()
        {
            var db = new TFContext(DatabaseSimulator());
            var genreServiceMock = new Mock<IGenreServices>();
            var movieService = new MovieServices(db, genreServiceMock.Object);

            Assert.ThrowsException<InexistingEntityException>
                (() => movieService.DeleteById(1));
        }
        [TestMethod]
        public void DeleteById_Should_DeleteTheMovieCorrect()
        {
            var db = new TFContext(DatabaseSimulator());
            var genreServiceMock = new Mock<IGenreServices>();
            var movieService = new MovieServices(db, genreServiceMock.Object);

            var movie = new Movie()
            {
                Id = 1,
                Title = "The Movie"
            };
            db.Movies.Add(movie);
            db.SaveChanges();

            Assert.IsFalse(movie.IsDeleted);

            movieService.DeleteById(1);

            Assert.IsTrue(movie.IsDeleted);
        }
        [TestMethod]
        public void Exists_Should_ReturnTrueWhenExistAndFalseWhenDoesNot()
        {
            var db = new TFContext(DatabaseSimulator());
            var genreServiceMock = new Mock<IGenreServices>();
            var movieService = new MovieServices(db, genreServiceMock.Object);

            var movie = new Movie()
            {
                Id = 1,
                Title = "The Movie"
            };
            bool doesNotExist =  movieService.Exists(1);
            db.Movies.Add(movie);
            db.SaveChanges();
            bool exist = movieService.Exists(1);

            Assert.IsFalse(doesNotExist);
            Assert.IsTrue(exist);

        }

        //Edit method
        private DbContextOptions<TFContext> DatabaseSimulator()
        {
            var dbOptions = new DbContextOptionsBuilder<TFContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return dbOptions;
        }
    }
}
