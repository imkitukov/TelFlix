using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TelFlix.App.Models;
using TelFlix.Services.Contracts;

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
            var movies = this.movieServices.ListAllMovies().ToList();
         
            return View(movies);
        }
        
        [HttpGet]
        public IActionResult SearchMovie()
        {
            var keyword = Request.Query["searchString"].ToString();

            var movies = this.movieServices.SearchMovie(keyword);

            this.ViewBag.SearchString = keyword;
           
            return View(movies);
        }

        //[HttpPost]
        //public IActionResult SearchMovie(string keyword)
        //{
        //    var movies = this.movieServices.SearchMovie(keyword);

        //    var vm = new SearchMovieViewModel { Movies = movies.Select(m => new MovieViewModel(m)) };

        //    return View(vm);
        //}

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
