using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using TelFlix.Data.Context;
using TelFlix.Data.Models;
using TelFlix.Services;

namespace TelFlix.Tests.Services
{
    [TestClass]
    public class ReviewServiceTests
    {
        [TestMethod]
        public void AddReview_Should_AddReviewCorrect()
        {
            var db = new TFContext(DatabaseSimulator());
            var userManager = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            var reviewServices = new ReviewService(db, userManager.Object);
            var user = new User()
            {
                Id = "UserId"
            };
            db.Users.Add(user);
            db.SaveChanges();
            reviewServices.AddReview("UserId", 1, "comment");

            Assert.AreEqual(1, db.Reviews.Count(a => a.Comment == "comment"));
        }
        [TestMethod]
        public void GetAllByMovieId_Should_WorkCorrect()
        {
            var db = new TFContext(DatabaseSimulator());
            var userManager = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            var reviewServices = new ReviewService(db, userManager.Object);
            var movie = new Movie()
            {
                Id = 1
            };
            var reviewOne = new Review() { MovieId = 1};
            var reviewTwo = new Review() { MovieId = 1};
            var reviewThree = new Review() { MovieId = 2 };
            db.Reviews.Add(reviewOne);
            db.Reviews.Add(reviewTwo);
            db.Reviews.Add(reviewThree);
            db.Movies.Add(movie);
            db.SaveChanges();

            Assert.AreEqual(2, reviewServices.GetAllByMovieId(1).Count());
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
