using TelFlix.Data.Models;

namespace TelFlix.Services.Contracts
{
    public interface IModifyMovieServices
    {
        string DeleteMovie(string movieTitle);

        string UpdateMovie(string movieTitle);

        Movie FindMovieByTitle(string movieTitle);
    }
}
