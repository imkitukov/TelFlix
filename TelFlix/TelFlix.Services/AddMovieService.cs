using System.Collections.Generic;
using System.Linq;
using TelFlix.Data.Context;
using TelFlix.Data.Models;
using TelFlix.Services.Abstract;
using TelFlix.Services.Contracts;
using TelFlix.Services.Providers.Exceptions;

namespace TelFlix.Services
{
    public class AddMovieService : BaseService, IAddMovieService
    {
        private readonly IGenreServices genreServices;
        private readonly IActorServices actorServices;

        public AddMovieService(TFContext context, IGenreServices genreServices, IActorServices actorServices)
            : base(context)
        {
            this.genreServices = genreServices;
            this.actorServices = actorServices;
        }

        public Movie AddMovie(Movie movie)
        {
            var movieToAdd = this.Context
                                 .Movies
                                 .FirstOrDefault(m => m.ApiMovieId == movie.ApiMovieId);

            if (movieToAdd != null)
            {
                if (movieToAdd.IsDeleted == false)
                {
                    throw new EntityAlreadyExistingException(nameof(Movie), movieToAdd.Title);
                }

                // restore the movie
                movieToAdd.IsDeleted = false;
            }
            else
            {
                // Add movie
                this.Context.Movies.Add(movie);
            }

            this.Context.SaveChanges();

            return movie;
        }

        public void AddGenresToMovie(Movie movie, IEnumerable<(int GenreApiId, string GenreName)> genres)
        {
            foreach (var currentGenre in genres)
            {
                var genre = this.Context.Genres.SingleOrDefault(g => g.ApiGenreId == currentGenre.GenreApiId);

                var moviesGenres = new MoviesGenres() { MovieId = movie.Id };

                if (genre != null)
                {
                    moviesGenres.GenreId = genre.Id;
                }
                else
                {
                    var addedGenre = this.genreServices
                                         .Add(new Genre
                                         {
                                             Name = currentGenre.GenreName,
                                             ApiGenreId = currentGenre.GenreApiId
                                         });

                    moviesGenres.GenreId = addedGenre.Id;
                }

                this.Context.MoviesGenres.Add(moviesGenres);
            }

            this.Context.SaveChanges();
        }

        public IEnumerable<Actor> AddActorsToMovie(
            Movie movie,
            IEnumerable<(Actor Actor, string MovieCharacter)> actorsCast)
        {
            var movieActors = new List<Actor>();

            foreach (var currentActor in actorsCast)
            {
                var actor = this.Context.Actors.SingleOrDefault(a => a.ApiActorId == currentActor.Actor.ApiActorId);

                var moviesActors = new MoviesActors()
                {
                    MovieId = movie.Id,
                    MovieCharacter = currentActor.MovieCharacter
                };

                if (actor != null)
                {
                    moviesActors.ActorId = actor.Id;
                    movieActors.Add(actor);
                }
                else
                {
                    var addedActor = this.actorServices
                                         .AddActor(currentActor.Actor);

                    moviesActors.ActorId = addedActor.Id;

                    movieActors.Add(addedActor);
                }

                this.Context.MoviesActors.Add(moviesActors);
            }

            this.Context.SaveChanges();

            return movieActors;
        }
    }
}