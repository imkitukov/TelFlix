using System.Collections.Generic;
using System.Linq;
using TelFlix.Data.Context;
using TelFlix.Data.Models;
using TelFlix.Services.Abstract;
using TelFlix.Services.Contracts;
using TelFlix.Services.Models.Genres;
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

        public IEnumerable<GenreModel> GetAll()
            => this.Context
                   .Genres
                   .Where(g => g.IsDeleted == false)
                   .OrderBy(g => g.Name)
                   .Select(g => new GenreModel
                   {
                       Id = g.Id,
                       Name = g.Name
                   })
                   .ToList();

        public bool GenreExists(string genreName) => this.Context
                       .Genres
                       .Any(g => g.Name == genreName);

        public void UpdateMovieGenres(int movieId, IEnumerable<int> selectedGenreIds, IEnumerable<int> genresIdsToRemove)
        {
            var movie = this.Context.Movies.Find(movieId);

            if (movie != null)
            {
                foreach (var selectedGenreId in selectedGenreIds)
                {
                    var existingRelation = this.Context
                                               .MoviesGenres
                                               .FirstOrDefault(mg =>
                                                                    mg.MovieId == movieId &&
                                                                    mg.GenreId == selectedGenreId);

                    if (existingRelation != null)
                    {
                        //Restore if deleted
                        if (existingRelation.IsDeleted == true)
                        {
                            existingRelation.IsDeleted = false;
                        }
                    }
                    else
                    {
                        this.Context.MoviesGenres.Add(new MoviesGenres
                        {
                            MovieId = movieId,
                            GenreId = selectedGenreId
                        });
                    }

                    this.Context.SaveChanges();
                }

                foreach (var genreToRemove in genresIdsToRemove)
                {
                    var existingRelation = this.Context
                                              .MoviesGenres
                                              .FirstOrDefault(mg =>
                                                                   mg.MovieId == movieId &&
                                                                   mg.GenreId == genreToRemove);

                    // delete genre
                    if (existingRelation != null)
                    {
                        existingRelation.IsDeleted = true;
                    }

                    this.Context.SaveChanges();
                }
            }
        }
    }
}
