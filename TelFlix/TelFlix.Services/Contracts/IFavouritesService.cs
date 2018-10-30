using System.Collections.Generic;
using TelFlix.Data.Models;
using TelFlix.Services.Models.Movie;

namespace TelFlix.Services.Contracts
{
    public interface IFavouritesService
    {
        IEnumerable<ListMovieModel> List(User user);

        Movie AddMovieToFavourite(string movieTitle, int userId);
    }
}
