using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TelFlix.App.Controllers;
using TelFlix.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using TelFlix.App.Models;

namespace TelFlix.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void IndexAction()
        {
            var controller = this.SetupController();

            var result = controller.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }



        private HomeController SetupController()
        {
            var movieServiceMock = new Mock<IMovieServices>();
            var genreServicesMock = new Mock<IGenreServices>();

            var controller = new HomeController
                (movieServiceMock.Object, genreServicesMock.Object);

            return controller;
        }
    }
}
