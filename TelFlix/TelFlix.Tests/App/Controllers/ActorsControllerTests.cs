using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TelFlix.App.Controllers;
using TelFlix.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using TelFlix.App.Models;
using TelFlix.Data.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TelFlix.Data.Context;
using System;
using TelFlix.Services;

namespace TelFlix.Tests.Controllers
{
    [TestClass]
    public class ActorsControllerTests
    {
        [TestMethod]
        public void IndexAction()
        {
            var controller = this.SetupController();

            var result = controller.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }


        private ActorsController SetupController()
        {
            var actorServiceMock = new Mock<IActorServices>();

            var controller = new ActorsController(actorServiceMock.Object);

            return controller;
        }
    }


    
}
