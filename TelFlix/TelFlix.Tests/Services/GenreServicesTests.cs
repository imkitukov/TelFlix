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
    public class GenreServicesTests
    {
        private const string genreNameToUse = "action";

        [TestMethod]
        public void Add_Should_ThrowWhenGenreAlreadyExists()
        {
            var db = new TFContext(this.DatabaseSimulator());
            var genreServices = new GenreServices(db);

            var genre = new Genre()
            {
                Name = genreNameToUse
            };

            db.Genres.Add(genre);
            db.SaveChanges();

            Assert.ThrowsException<EntityAlreadyExistingException>
                (() => genreServices.Add(genre));
        }
        [TestMethod]
        public void Add_Should_AddCorrect()
        {
            var db = new TFContext(this.DatabaseSimulator());
            var genreServices = new GenreServices(db);

            var genre = new Genre()
            {
                Name = genreNameToUse
            };

            genreServices.Add(genre);

            Assert.AreEqual(1, db.Genres.Count());
        }
        [TestMethod]
        public void GenreExist_Should_ReturnTrueWhenExistAndNoWhenDoesNot()
        {
            var db = new TFContext(this.DatabaseSimulator());
            var genreServices = new GenreServices(db);

            var genre = new Genre()
            {
                Name = genreNameToUse
            };
            bool doNotExist = genreServices.GenreExists(genreNameToUse);
            db.Genres.Add(genre);
            db.SaveChanges();


            bool exist = genreServices.GenreExists(genreNameToUse);

            Assert.IsFalse(doNotExist);
            Assert.IsTrue(exist);
        }
        [TestMethod]
        public void GetAll_Should_GetAll()
        {
            var db = new TFContext(this.DatabaseSimulator());
            var genreServices = new GenreServices(db);

            var genre = new Genre()
            {
                Name = genreNameToUse
            };
            var secondGenre = new Genre()
            {
                Name = "drama"
            };
            var thirdGenre = new Genre()
            {
                Name = "animation"
            };
            var deletedGenre = new Genre()
            {
                Name = "comedy",
                IsDeleted = true

            };
            db.Genres.Add(genre);
            db.Genres.Add(secondGenre);
            db.Genres.Add(thirdGenre);
            db.Genres.Add(deletedGenre);
            db.SaveChanges();

            int getAllMethod = genreServices.GetAll().Count();

            Assert.AreEqual(3, getAllMethod);


        }


        //UpdateMovieGenres Method
        private DbContextOptions<TFContext> DatabaseSimulator()
        {
            var dbOptions = new DbContextOptionsBuilder<TFContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return dbOptions;
        }
    }
}
