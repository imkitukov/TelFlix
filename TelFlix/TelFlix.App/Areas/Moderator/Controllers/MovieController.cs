using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TelFlix.App.Areas.Moderator.Models.Movie;
using TelFlix.Services.Contracts;

namespace TelFlix.App.Areas.Moderator.Controllers
{
    [Area("Moderator")]
    //[Authorize(Roles = "Moderator")]
    public class MovieController : Controller
    {
        private readonly IMovieServices movieServices;
        private readonly IGenreServices genresServices;

        public MovieController(IMovieServices movieServices, IGenreServices genresServices)
        {
            this.movieServices = movieServices;
            this.genresServices = genresServices;
        }

        // GET: Moderator/Movie/Edit/5
        public IActionResult Edit(int id)
        {
            var movie = this.movieServices.GetMovieById(id);

            if (movie == null)
            {
                return NotFound();
            }

            var allGenres = this.genresServices.GetAll();

            var genresCheckBoxes = allGenres.Select(g => new GenreCheckBox
            {
                Id = g.Id,
                Name = g.Name,
                Selected = (movie.Genres.Any(genre => genre.Name == g.Name)) ? true : false
            }).ToArray();

            var vm = new EditMovieFormModel
            {
                Title = movie.Title,
                DurationInMinutes = movie.DurationInMinutes,
                TrailerUrl = movie.TrailerUrl,
                Description = movie.Description,
                Genres = genresCheckBoxes
            };

            return View(vm);
        }

        // POST: Movie/Edit/5
        [HttpPost]
        public IActionResult Edit(int id, EditMovieFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newGenresId = model.Genres.Where(g => g.Selected == true).Select(g => g.Id);
            var genresIdsToRemove = model.Genres.Where(g => g.Selected == false).Select(g => g.Id);

            this.movieServices.Edit(
                id,
                model.Title,
                model.Description,
                model.DurationInMinutes,
                model.TrailerUrl,
                newGenresId,
                genresIdsToRemove);

            return RedirectToAction("Details", "Movies", new { Area = "", id = id });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var movieExists = this.movieServices.Exists(id);

            if (!movieExists)
            {
                return NotFound();
            }

            this.movieServices.DeleteById(id);

            return RedirectToAction("Index", "Movies", new { area = "" });
        }
    }
}