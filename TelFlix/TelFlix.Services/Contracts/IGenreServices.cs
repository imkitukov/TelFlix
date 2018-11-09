using System.Collections.Generic;
using TelFlix.Data.Models;
using TelFlix.Services.Models.Genres;

namespace TelFlix.Services.Contracts
{
    public interface IGenreServices
    {
        Genre Add(Genre genre);

        Genre FindByName(string genreName);

        IEnumerable<GenreModel> GetAll();

        bool GenreExists(string genreName);

        void UpdateMovieGenres(int movieId, IEnumerable<int> selectedGenreIds, IEnumerable<int> genresIdsToRemove);
    }
}
