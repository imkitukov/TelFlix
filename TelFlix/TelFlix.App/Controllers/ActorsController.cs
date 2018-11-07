using TelFlix.Services.Contracts;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TelFlix.Services.Models.Actors;
using System.Linq;
using TelFlix.App.Models;

namespace TelFlix.App.Controllers
{
    public class ActorsController : Controller
    {
        private readonly IActorServices actorServices;

        public ActorsController(IActorServices actorServices)
        {
            this.actorServices = actorServices;
        }

        public IActionResult Index()
        {
            var actor = actorServices.ListAllActors();

            return View(actor);
        }

        public IActionResult Details(int id)
        {
            var actor = actorServices.FindActorById(id);

            var model = new ActorDetailsViewModel
            {
                FullName = actor.FullName,
                MediumImageUrl = actor.MediumImageUrl,
                ApiActorId = actor.ApiActorId,
                Biography = actor.Biography,
                DateOfBirth = actor.DateOfBirth,
                ImdbProfileUrl = actor.ImdbProfileUrl,
                PlaceOfBirth = actor.PlaceOfBirth,
                Movies = actor.Movies,
            };

            return View(model);
        }
    }
}
