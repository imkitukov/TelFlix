using System.Threading.Tasks;

namespace TelFlix.App.HttpClients
{
    public interface ITheMovieDbClient
    {
        Task<string> GetActorDetails(int actorId);
        Task<string> GetMovieActors(int movieId);
        Task<string> GetMovieDetails(int movieId);
        Task<string> ListAllGenres();
        Task<string> SearchMovie(string searchTerm);
    }
}