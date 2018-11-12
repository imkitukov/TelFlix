using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;
using TelFlix.App.HttpClients;
using TelFlix.App.Infrastructure.Providers;
using TelFlix.App.Models;
using TelFlix.App.Models.Movies;
using TelFlix.Data.Models;
using TelFlix.Services.Contracts;
using TelFlix.Services.Providers.Exceptions;

namespace TelFlix.App.Controllers
{
    public class MoviesController : Controller
    {
        private const int PageSize = 5;

        private readonly ITheMovieDbClient client;
        private readonly IJsonProvider jsonProvider;
        private readonly IFavouritesService favouritesService;
        private readonly UserManager<User> userManager;
        private readonly IAddMovieService addMovieService;
        private readonly IMovieServices movieService;
        private readonly IActorServices actorServices;
        private readonly IGenreServices genresServices;

        public MoviesController(IAddMovieService addMovieService, IMovieServices movieServices,
            IActorServices actorServices, IGenreServices genreServices,
            ITheMovieDbClient client, IJsonProvider jsonProvider, IFavouritesService favouritesService, UserManager<User> userManager)
        {
            this.client = client;
            this.jsonProvider = jsonProvider;
            this.favouritesService = favouritesService;
            this.userManager = userManager;
            this.addMovieService = addMovieService;
            this.movieService = movieServices;
            this.actorServices = actorServices;
            this.genresServices = genreServices;
        }

        // GET: Movies
        public IActionResult Index()
        {
            var genres = this.genresServices.GetAll();

            var vm = new MovieIndexViewModel
            {
                Genres = new SelectList(genres, "Id", "Name"),
                GenreId = 0,
                SelectMovieResultViewModel = this.UpdateMovieIndexViewModel()
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult GetGenreMovies(string genre, int page = 1)
        {
            var genreId = int.Parse(genre);
            var model = this.UpdateMovieIndexViewModel(page, genreId);

            return PartialView("_GenreResults", model);
        }

        private SelectMovieResultViewModel UpdateMovieIndexViewModel(int page = 1, int genreId = 0)
        {
            var movies = this.movieService.GetAllByGenre(genreId, page, PageSize);
            var totalMoviesInGenre = this.movieService.TotalMoviesInGenre(genreId);

            var vm = new SelectMovieResultViewModel()
            {
                Movies = movies,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalMoviesInGenre / (double)PageSize)
            };

            return vm;
        }

        // search movie at The Movie DB
        [HttpPost]
        public async Task<IActionResult> Search(SearchMovieViewModel model)
        {
            var jsonResult = await this.client.SearchMovie(model.SearchString);

            try
            {
                var apiMoviesFound = this.jsonProvider.ExtractFoundMoviesFromSearchMovieJsonResult(jsonResult);
                model.ApiMovies = apiMoviesFound.Select(m => new MovieViewModel(m));

                var dbMoviesFound = this.movieService.SearchMovie(model.SearchString);
                model.DbMovies = dbMoviesFound.Select(m => new MovieViewModel(m)); ;
            }
            catch (InvalidOperationException)
            {              
            }
         
            return View(model);
        }

        // GET: Movies/AddToDb/{id}
        public async Task<IActionResult> AddToDb(int id)
        {
            var jsonResult = await this.client.GetMovieDetails(id);
            Movie addedMovie;

            // Add movie to db
            try
            {
                var movie = this.jsonProvider.ExtractMovieFromMovieDetailsJsonResult(jsonResult);
                addedMovie = this.addMovieService.AddMovie(movie);
            }
            catch (ResourceNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (EntityAlreadyExistingException e)
            {
                return BadRequest(e.Message);
            }

            // Add movie genres to the relation many to many table
            try
            {
                var movieGenresApiIds = this.jsonProvider.ExtractGenresForMovie(jsonResult);
                this.addMovieService.AddGenresToMovie(addedMovie, movieGenresApiIds);
            }
            catch (ResourceNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (EntityAlreadyExistingException e)
            {
                return BadRequest(e.Message);
            }

            var castJsonResult = await this.client.GetMovieActors(id);

            // Add movie actors to relation many to many table
            try
            {
                var actorsCast = this.jsonProvider.ExtractActorsFromMovieCastJsonResult(castJsonResult);

                var movieActors = this.addMovieService.AddActorsToMovie(addedMovie, actorsCast);

                //add actor details
                foreach (var actor in movieActors)
                {
                    var actorDetailsJsonResult = await this.client.GetActorDetails(actor.ApiActorId);

                    var actorDetails = this.jsonProvider.ExtractActorDetails(actorDetailsJsonResult);

                    actorDetails.Id = actor.Id;

                    this.actorServices.AddActorDetails(actorDetails);
                }

            }
            catch (ResourceNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (EntityAlreadyExistingException e)
            {
                return BadRequest(e.Message);
            }

            return NoContent();
            //return RedirectToAction(nameof(this.Details), new { id = addedMovie.Id });
        }

        // GET: Movies/Details/5
        public IActionResult Details(int id, string returnUrl = "")
        {
            var vm = this.movieService.GetMovieById(id);

            if (this.User.Identity.IsAuthenticated)
            {
                var userId = this.userManager.FindByEmailAsync(this.User.Identity.Name).Result.Id;

                var isInLibrary = this.favouritesService.IsInLibrary(id, userId);

                if (vm == null)
                {
                    return NotFound();
                }

                vm.IsInLibrary = isInLibrary;
            }

            if (returnUrl == string.Empty)
            {
                return View(vm);
            }
            else
            {
                return Redirect(returnUrl);
            }
        }
    }
}
