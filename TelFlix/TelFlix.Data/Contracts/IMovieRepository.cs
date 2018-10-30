using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelFlix.Data.Models;
using TelFlix.Data.Repository;

namespace TelFlix.Data.Contracts
{
    public interface IMovieRepository : IRepository<Movie>
    {
        IEnumerable<Movie> GetAllWithActors();

        IEnumerable<Movie> GetAllWithGenres();

    }
}
