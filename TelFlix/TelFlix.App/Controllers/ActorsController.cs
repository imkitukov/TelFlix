using Microsoft.AspNetCore.Mvc;
using System;
using TelFlix.App.Models;
using TelFlix.App.Models.Actors;
using TelFlix.Services.Contracts;

namespace TelFlix.App.Controllers
{
    public class ActorsController : Controller
    {
        private const int PageSize = 10;

        private readonly IActorServices actorServices;

        public ActorsController(IActorServices actorServices)
        {
            this.actorServices = actorServices;
        }

        public IActionResult Index(int page = 1)
        {
            var actors = actorServices.ListAllActors(page, PageSize);
            var totalActors = actorServices.Count();

            var vm = new ActorPageListingViewModel
            {
                Actors = actors,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalActors / (double)PageSize),
                Search = ""
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Index(ActorPageListingViewModel model, int page = 1)
        {
            var actors = actorServices.ListAllActors(page, PageSize, model.Search);
            var totalActors = actorServices.Count(model.Search);

            var vm = new ActorPageListingViewModel
            {
                Actors = actors,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalActors / (double)PageSize),
            };

            return View(vm);
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
