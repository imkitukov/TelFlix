using TelFlix.Data.Models;

namespace TelFlix.Services.Contracts
{
    public interface IDirectorServices
    {
        string AddDirector(string[] directors, Movie movie);
    }
}
