

using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using TelFlix.App.Controllers;
using TelFlix.Data.Context;
using TelFlix.Data.Models;
using TelFlix.Services;
using TelFlix.Services.Contracts;
using TelFlix.Services.Providers.Exceptions;

namespace TelFlix.Tests.Services
{

    [TestClass]
    public class ActorServiceTests
    {
        private const string firstActorName = "Pesho Peshov";
        private const string secondActorName = "Pesho Peshkov";
        private const string nameWithoutP = "Esho Eshkov";
        private const int actorId = 1;

        [TestMethod]
        public void FindActorByIdShould_FindActorCorrect()
        {
            var db = new TFContext(this.DatabaseSimulator());
            var actorService = new ActorServices(db);

            var actor = new Actor()
            {
                Id = actorId,
                FullName = firstActorName
            };
            db.Actors.Add(actor);
            db.SaveChanges();

            var result = actorService.FindActorById(1);
            Assert.AreEqual(firstActorName, result.FullName);
        }
        [TestMethod]
        public void FindActorByNameShould_FindActorCorrect()
        {
            var db = new TFContext(this.DatabaseSimulator());
            var actorService = new ActorServices(db);

            var actor = new Actor()
            {
                Id = actorId,
                FullName = firstActorName
            };
            db.Actors.Add(actor);
            db.SaveChanges();

            var result = actorService.FindActorByName(firstActorName);
            Assert.AreEqual(1, result.Id);
        }
        [TestMethod]
        public void AddActorShould_AddActorCorrect()
        {
            var db = new TFContext(this.DatabaseSimulator());
            var actorService = new ActorServices(db);

            var actor = new Actor()
            {
                Id = actorId,
                FullName = firstActorName
            };

            var result = actorService.AddActor(actor);

            Assert.AreEqual(1, result.Id);
        }
        [TestMethod]
        public void AddActorShould_ThrowWhenActorExist()
        {

            var db = new TFContext(this.DatabaseSimulator());
            var actorService = new ActorServices(db);

            Actor actorPesho = new Actor
            {
                ApiActorId = actorId,
                FullName = firstActorName

            };

            actorService.AddActor(actorPesho);

            Assert.ThrowsException<EntityAlreadyExistingException>(() => actorService.AddActor(actorPesho));
        }
        [TestMethod]
        public void AddActorShould_RestoreActor()
        {
            var db = new TFContext(this.DatabaseSimulator());
            var actorService = new ActorServices(db);

            Actor actorPesho = new Actor
            {
                ApiActorId = actorId,
                FullName = firstActorName
            };

            actorService.AddActor(actorPesho);

            actorPesho.IsDeleted = true;

            actorService.AddActor(actorPesho);

            Assert.IsFalse(actorPesho.IsDeleted);
        }
        [TestMethod]
        public void AddActorDetails_ShouldAddDetails()
        {
            var db = new TFContext(this.DatabaseSimulator());
            var actorService = new ActorServices(db);

            var actorPesho = new Actor()
            {
                Id = actorId
            };
            var actorWithDetails = new Actor()
            {
                Id = actorId,
                DateOfBirth = "Some nice day",
                Biography = "Some nice words"
            };

            db.Actors.Add(actorPesho);
            db.SaveChanges();


            Assert.IsNull(actorPesho.Biography);
            Assert.IsNull(actorPesho.DateOfBirth);

            actorService.AddActorDetails(actorWithDetails);

            Assert.IsNotNull(actorPesho.Biography);
            Assert.IsNotNull(actorPesho.DateOfBirth);
        }
        [TestMethod]
        public void _ListAllActors_ShouldReturnActorsWithCorrectName()
        {
            var db = new TFContext(this.DatabaseSimulator());
            var actorService = new ActorServices(db);

            var firstActor = new Actor()
            {
                Id = 1,
                FullName = firstActorName
            };
            var secondActor = new Actor()
            {
                Id = 2,
                FullName = secondActorName
            };
            var thirdActor = new Actor()
            {
                Id = 3,
                FullName = nameWithoutP
            };
            db.Actors.AddRange(firstActor, secondActor, thirdActor);
            db.SaveChanges();
            var result = actorService.ListAllActors(1, 2, "Pesho").ToList();

            Assert.AreEqual(2, result.Count());
        }
        [TestMethod]
        public void _ListAllActors_ShouldReturnAllActorsWithWhiteSpace()
        {
            var db = new TFContext(this.DatabaseSimulator());
            var actorService = new ActorServices(db);

            var firstActor = new Actor()
            {
                Id = 1,
                FullName = firstActorName
            };
            var secondActor = new Actor()
            {
                Id = 2,
                FullName = secondActorName
            };
            var thirdActor = new Actor()
            {
                Id = 3,
                FullName = nameWithoutP
            };
            db.Actors.AddRange(firstActor, secondActor, thirdActor);
            db.SaveChanges();
            var result = actorService.ListAllActors(1, 5, " ").ToList();

            Assert.AreEqual(3, result.Count());
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
