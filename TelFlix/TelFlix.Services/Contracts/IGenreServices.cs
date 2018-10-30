using System.Collections.Generic;
using TelFlix.Data.Models;

namespace TelFlix.Services.Contracts
{
    public interface IGenreServices
    {
        Genre Add(Genre genre);

        Genre FindByName(string genreName);

        IEnumerable<Genre> GetAll();

        bool GenreExists(string genreName);
    }
}
