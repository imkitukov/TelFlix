using System.Collections.Generic;
using TelFlix.Data.Models;

namespace TelFlix.App.Infrastructure.Providers
{
    public interface IJsonProvider
    {
        IEnumerable<Movie> ExtractFoundMoviesFromSearchMovieJsonResult(string jsonAsString);

        Movie ExtractMovieFromMovieDetailsJsonResult(string jsonAsString);

        IEnumerable<Genre> ExtractGenresFromListAllGenresJsonResult(string jsonAsString);

        IEnumerable<(int GenreApiId, string GenreName)> ExtractGenresForMovie(string jsonAsString);

        IEnumerable<(Actor Actor, string MovieCharacter)> ExtractActorsFromMovieCastJsonResult(string jsonAsString);

        Actor ExtractActorDetails(string actorDetailsJsonResult);
    }
}
