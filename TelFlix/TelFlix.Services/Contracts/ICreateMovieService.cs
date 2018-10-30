using Newtonsoft.Json.Linq;
using TelFlix.Data.Models;

namespace TelFlix.Services.Contracts
{
    public interface ICreateMovieService
    {
        (string Title, Movie Movie) CreateMovie(JObject movieJson);
    }
}
