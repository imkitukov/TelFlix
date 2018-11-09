using System.Collections.Generic;
using TelFlix.Data.Models;
using TelFlix.Services.Models.Movie;

namespace TelFlix.Services.Contracts
{
    public interface IMovieServices
    {
        MovieDetailModel GetMovieById(int id);

        IEnumerable<ListMovieModel> ListAllMovies(int genreId = 0, int page = 1, int pageSize = 3);

        IEnumerable<Movie> SearchMovie(string searchString);

        IEnumerable<Movie> ListTopTenMovies();

        int Count();

        int TotalMoviesInGenre(int genreId);

        void Edit(
            int id,
            string title,
            string description,
            int? durationInMinutes,
            string trailerUrl,
            IEnumerable<int> selectedGenresIds,
            IEnumerable<int> genresIdsToRemove);

        //IEnumerable<ListMoviesByGenreViewModel> ListMoviesByGenre(string stringToSearch);

        //IEnumerable<ListGenresViewModel> ListGenres();

        //IEnumerable<ListMovieViewModel> ListMoviesInRange(int firstYear, int secondYear);
    }
}
