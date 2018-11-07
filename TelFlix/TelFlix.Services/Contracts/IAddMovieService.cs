using System.Collections.Generic;
using TelFlix.Data.Models;

namespace TelFlix.Services.Contracts
{
    public interface IAddMovieService
    {
        Movie AddMovie(Movie movie);

        void AddGenresToMovie(Movie movie, IEnumerable<(int GenreApiId, string GenreName)> genresApiIds);

        IEnumerable<Actor> AddActorsToMovie(Movie movie, IEnumerable<(Actor Actor, string MovieCharacter)> actorsCast);
    }
}
