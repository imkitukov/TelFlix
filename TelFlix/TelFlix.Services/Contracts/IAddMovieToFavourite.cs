using TelFlix.Data.Models;
namespace TelFlix.Services.Contracts
{
    public interface IAddMovieToFavourite
    {
        void AddMovieToFavourite(string movieTitle, int userId);
    }
}
