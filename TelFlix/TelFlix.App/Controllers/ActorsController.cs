using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
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
            };

            return View(vm);
        }

        public IActionResult Details(int id)
        {
            var actor = actorServices.FindActorById(id);

            var model = new ActorViewModel
            {
                FullName = actor.FullName,
                MediumImageUrl = actor.MediumImageUrl
            };

            return View(model);
        }
    }
}
