using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TelFlix.Data.Context;
using TelFlix.Data.Contracts;
using TelFlix.Data.Models;

namespace TelFlix.Data.Repository
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(ITFContext context)
            : base(context)
        {
        }

        public IEnumerable<Movie> GetAllWithActors()
        {
            return this.Context.Movies
                               .Include(m => m.MoviesActors)
                                    .ThenInclude(ma => ma.Actor);
        }

        public IEnumerable<Movie> GetAllWithGenres()
        {
            return this.Context.MoviesGenres
                                    .Include(g => g.Movie)
                                    .Select(mg => mg.Movie);
        }
    }
}
