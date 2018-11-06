using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelFlix.App.HttpClients;
using TelFlix.App.Infrastructure.Providers;
using TelFlix.App.Models;
using TelFlix.App.Models.Movies;
using TelFlix.Data.Models;
using TelFlix.Data.UnitOfWorkCore;
using TelFlix.Services.Contracts;
using TelFlix.Services.Providers.Exceptions;

namespace TelFlix.App.Controllers
{
    public class MoviesController : Controller
    {
        private readonly TheMovieDbClient client;
        private readonly IJsonProvider jsonProvider;
        private readonly IAddMovieService addMovieService;
        private readonly IMovieServices movieService;
        private readonly IActorServices actorServices;
        private readonly IGenreServices genresServices;

        public MoviesController(IAddMovieService addMovieService, IMovieServices movieServices,
            IActorServices actorServices, IGenreServices genreServices,
            TheMovieDbClient client, IJsonProvider jsonProvider)
        {
            this.client = client;
            this.jsonProvider = jsonProvider;
            this.addMovieService = addMovieService;
            this.movieService = movieServices;
            this.actorServices = actorServices;
            this.genresServices = genreServices;
        }

        // GET: Movies
        public IActionResult Index()
        {
            var vm = this.GetFullAndPartialViewModel();

            return View(vm);
        }

        [HttpGet]
        public IActionResult GetGenreMovies(string genreId)
        {
            var lookupId = int.Parse(genreId);
            var model = this.GetFullAndPartialViewModel(lookupId);
            return PartialView("_GenreResults", model);
        }

        private MovieIndexViewModel GetFullAndPartialViewModel(int genreId = 0)
        {
            // add service method GetMoviesByGenre
            var movies = this.movieService.ListAllMovies();
            var genres = this.genresServices.GetAll();

            var vm = new MovieIndexViewModel();
            vm.Movies = movies.Where(m => m.Genres.Any(g => g.Id == genreId));
            //if (genreId != 0)
            //{
            //    movies = movies.Where(m => m.Id == genreId);
            //}

            //vm.Movies = movies;
            vm.Genres = new SelectList(genres, "Id", "Name");

            //return vm;
            return vm;
        }
        // search movie at The Movie DB
        [HttpPost]
        public async Task<IActionResult> Search(SearchMovieViewModel model)
        {
            var jsonResult = await this.client.SearchMovie(model.SearchString);

            var apiMoviesFound = this.jsonProvider.ExtractFoundMoviesFromSearchMovieJsonResult(jsonResult);

            model.ApiMovies = apiMoviesFound.Select(m => new MovieViewModel(m));

            var dbMoviesFound = this.movieService.SearchMovie(model.SearchString);

            model.DbMovies = dbMoviesFound.Select(m => new MovieViewModel(m)); ;

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
                this.addMovieService.AddActorsToMovie(addedMovie, actorsCast);
            }
            catch (ResourceNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (EntityAlreadyExistingException e)
            {
                return BadRequest(e.Message);
            }

            return RedirectToAction(nameof(this.Details), new { id = addedMovie.Id });
            //return View(await _context.Movies.ToListAsync());
        }

        // GET: Movies/Details/5
        public IActionResult Details(int id)
        {
            var movie = this.movieService.GetMovieById(id);
            
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }


        //// GET: Movies/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var movie = await _context.Movies.FindAsync(id);
        //    if (movie == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(movie);
        //}

        //// POST: Movies/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Title,ReleaseDate,DurationInMinutes,Rating,Description,Id,IsDeleted,DeletedOn,CreatedOn,ModifiedOn")] Movie movie)
        //{
        //    if (id != movie.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(movie);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!MovieExists(movie.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(movie);
        //}

        //// GET: Movies/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var movie = await _context.Movies
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (movie == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(movie);
        //}

        //// POST: Movies/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var movie = await _context.Movies.FindAsync(id);
        //    _context.Movies.Remove(movie);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

    }
}
