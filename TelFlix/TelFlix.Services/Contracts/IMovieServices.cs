using System.Collections.Generic;
using TelFlix.Data.Models;
using TelFlix.Services.Models.Movie;

namespace TelFlix.Services.Contracts
{
    public interface IMovieServices
    {
        Movie GetMovieById(int id);

        IEnumerable<ListMovieModel> ListAllMovies();

        IEnumerable<Movie> SearchMovie(string searchString);

        IEnumerable<Movie> ListTopTenMovies();

        //IEnumerable<ListMoviesByGenreViewModel> ListMoviesByGenre(string stringToSearch);

        //IEnumerable<ListGenresViewModel> ListGenres();

        //IEnumerable<ListMovieViewModel> ListMoviesInRange(int firstYear, int secondYear);
    }
}
