using System.Collections.Generic;
using System.Threading.Tasks;
using TelFlix.Data.Models;
using TelFlix.Services.Models.Movie;

namespace TelFlix.Services.Contracts
{
    public interface IMovieServices
    {
        MovieDetailModel GetMovieById(int id);

        IEnumerable<ListMovieModel> GetAllByGenre(int genreId = 0, int page = 1, int pageSize = 3);

        IEnumerable<Movie> SearchMovie(string searchString);

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

        IEnumerable<TopListMovieModel> GetTop5ByRating();

        void DeleteById(int id);

        bool Exists(int id);

        bool ApiIdExists(int apiId);

        Task<(int Id, string Title)> GetMovieByApiId(int addedMovieApiId);
    }
}
