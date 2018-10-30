using System.Collections.Generic;
using System.Linq;
using TelFlix.Data.Context;
using TelFlix.Data.Models;
using TelFlix.Services.Abstract;
using TelFlix.Services.Contracts;
using TelFlix.Services.Providers.Exceptions;

namespace TelFlix.Services
{
    public class GenreServices : BaseService, IGenreServices
    {
        public GenreServices(TFContext context) : base(context)
        {
        }

        public Genre Add(Genre genre)
        {
            if (this.GenreExists(genre.Name))
            {
                throw new EntityAlreadyExistingException(nameof(Genre), genre.Name, "database");
            }

            this.Context
                .Genres
                .Add(genre);

            this.Context.SaveChanges();

            return genre;
        }

        public Genre FindByName(string genreName) => this.Context
                       .Genres
                       .FirstOrDefault(g => g.Name == genreName);

        public IEnumerable<Genre> GetAll() => this.Context.Genres.ToList();

        public bool GenreExists(string genreName) => this.Context
                       .Genres
                       .Any(g => g.Name == genreName);
    }
}
