
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TelFlix.App.Controllers;
using TelFlix.App.HttpClients;
using TelFlix.App.Infrastructure.Providers;
using TelFlix.Services;
using TelFlix.Services.Contracts;

namespace TelFlix.Tests.App.ControllersTests
{
    [TestClass]
    public class Movies
    {
        [TestMethod]
        public void A()
        {

            var clientMock = new Mock<TheMovieDbClient>();
            var jsonProviderMock = new Mock<IJsonProvider>();
            var addMovieServiceMock = new Mock<IAddMovieService>();
            var movieServiceMock = new Mock<IMovieServices>();
            var actorServicesMock = new Mock<IActorServices>();
            var genreServiceMock = new Mock<GenreServices>();

            genreServiceMock.Setup(a => a.GetAll());

            var contoller = new MoviesController(addMovieServiceMock.Object, movieServiceMock.Object,
            actorServicesMock.Object, genreServiceMock.Object,
            clientMock.Object, jsonProviderMock.Object);

            var result = contoller.Index();
            
        }
    }
}
