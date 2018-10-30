using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TelFlix.App.HttpClients;
using TelFlix.App.Infrastructure.Providers;
using TelFlix.Services.Contracts;
using TelFlix.Services.Providers.Exceptions;

namespace TelFlix.App.Controllers
{
    public class GenresController : Controller
    {
        private readonly TheMovieDbClient client;
        private readonly IJsonProvider jsonProvider;
        private readonly IGenreServices genreService;

        public GenresController(TheMovieDbClient client, IJsonProvider jsonProvider, IGenreServices genreService)
        {
            this.client = client;
            this.jsonProvider = jsonProvider;
            this.genreService = genreService;
        }

        // action for admin to seed all genres
        public async Task<IActionResult> SeedAllGenresToDb()
        {
            var jsonResult = await this.client.ListAllGenres();
            var apiExistingGenres = this.jsonProvider.ExtractGenresFromListAllGenresJsonResult(jsonResult);

            foreach (var genre in apiExistingGenres)
            {
                try
                {
                    this.genreService.Add(genre);
                }
                catch (EntityAlreadyExistingException e)
                {
                    // log exc message to logger ?
                    return BadRequest(e.Message);
                }
            }

            // TODO : Review what to return
            //      Created - 201
            return NoContent();
        }

    }
}
