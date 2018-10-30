using TelFlix.Data.Context;
using TelFlix.Data.Contracts;
using TelFlix.Data.Models;

namespace TelFlix.Data.Repository
{
    public class GenreRepository : Repository<Genre>, IGenreRepository
    {
        public GenreRepository(ITFContext context) : base(context)
        {

        }


    }
}
