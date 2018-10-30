using TelFlix.Data.Models;

namespace TelFlix.Data.Contracts
{
    public interface IActorRepository
    {
        Actor GetWithMovies(int id);
    }
}
