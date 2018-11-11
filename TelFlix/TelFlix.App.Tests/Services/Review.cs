using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using TelFlix.Data.Context;
using TelFlix.Data.Models;

namespace TelFlix.Tests.Services
{
    [TestClass]
    public class ReviewTests
    {
        [TestMethod]
        public void A()
        {
            var contextMock = new Mock<ITFContext>();

            contextMock
                .Setup(a => a.Reviews.Add(new Review()))
                .Returns();
        }
    }
}
