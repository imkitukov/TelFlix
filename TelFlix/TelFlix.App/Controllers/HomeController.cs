using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Diagnostics;
using TelFlix.App.Models;
using TelFlix.Services.Contracts;

namespace TelFlix.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMovieServices movieServices;
        private readonly IGenreServices genreServices;
        private readonly IMemoryCache memoryCache;

        public HomeController(IMovieServices movieServices, IGenreServices genreServices, IMemoryCache memoryCache)
        {
            this.movieServices = movieServices;
            this.genreServices = genreServices;
            this.memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            var movies = this.movieServices.GetTop5ByRating();
            var moviesCacheEntry = this.memoryCache.GetOrCreate("TopFiveMoviesByRating", entry =>
            {
                entry.AbsoluteExpiration = DateTime.UtcNow.AddMinutes(15);
                return this.movieServices.GetTop5ByRating();
            });

            return View(movies);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult SeedDb()
        {
            ViewData["Message"] = "Data seeded!";

            //this.seedDatabaseService.SeedAsync();

            return RedirectToAction("Index");
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
