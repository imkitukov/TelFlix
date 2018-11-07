using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using TelFlix.App.Models;
using TelFlix.Services.Contracts;
using TelFlix.Services.Models.Movie;

namespace TelFlix.App.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ISeedDatabaseService seedDatabaseService;
        private readonly IMovieServices movieServices;
        private readonly IGenreServices genreServices;

        public HomeController(IMovieServices movieServices, IGenreServices genreServices)
        {
            //this.seedDatabaseService = seedDatabaseService;
            this.movieServices = movieServices;
            this.genreServices = genreServices;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult SearchMovie()
        {
            var keyword = Request.Query["searchString"].ToString();

            var movies = this.movieServices.SearchMovie(keyword);

            return View();
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
