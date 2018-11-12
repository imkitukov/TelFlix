using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using TelFlix.Data.Context;
using TelFlix.Data.Models;
using TelFlix.Services;

namespace TelFlix.Tests.Services
{
    [TestClass]
    public class MessageServiceTests
    {
        [TestMethod]
        public void AddMessage_Should_AddMessage()
        {
            var db = new TFContext(DatabaseSimulator());
            var messageService = new MessageServices(db);
            var message = new Message();

            messageService.AddMessage(message);

            Assert.AreEqual(1, db.Messages.Count());
        }
        [TestMethod]
        public void DeleteMessage_Should_Delete()
        {
            var db = new TFContext(DatabaseSimulator());
            var messageService = new MessageServices(db);
            var message = new Message()
            {
                Id = 1
            };
            var secondMessage = new Message()
            {
                Id = 2
            };
            db.Messages.AddRange(message, secondMessage);

            messageService.DeleteMessage(2);

            Assert.AreEqual(1, db.Messages.Count());
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
