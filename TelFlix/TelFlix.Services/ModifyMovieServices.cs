using System.Linq;
using TelFlix.Data.Context;
using TelFlix.Data.Models;
using TelFlix.Services.Abstract;
using TelFlix.Services.Contracts;
using TelFlix.Services.Providers.Exceptions;

namespace TelFlix.Services
{
    public class ModifyMovieServices : BaseService, IModifyMovieServices
    {
        private const string SuccessfullDeleteMessage = "Successful deleted movie with Title: {0} !";

        public ModifyMovieServices(TFContext context) : base(context)
        {
        }

        public string DeleteMovie(string movieTitle)
        {
            var movie = this.FindMovieByTitle(movieTitle);

            //this.Context.Movies.Delete(movie);

            this.Context.SaveChanges();

            return string.Format(SuccessfullDeleteMessage, movieTitle);
        }

        public string UpdateMovie(string movieTitle)
        {
            throw new System.NotImplementedException();
        }

        public Movie FindMovieByTitle(string movieTitle)
        {
            var movie = this.Context
                            .Movies
                            .FirstOrDefault(m => m.Title == movieTitle);

            if (movie == null)
            {
                throw new InexistingEntityException(nameof(Movie), movieTitle);
            }

            return movie;
        }
    }
}
