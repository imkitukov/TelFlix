using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TelFlix.Data.Models;
using TelFlix.Services.Providers.Exceptions;

namespace TelFlix.App.Infrastructure.Providers
{
    public class JsonProvider : IJsonProvider
    {
        private const string YouTubeBaseUri = "https://www.youtube.com/embed/";
        private const string SmallImageBaseUri = "http://image.tmdb.org/t/p/w185";
        private const string MediumImageBaseUri = "http://image.tmdb.org/t/p/w342";
        private const string LargeImageBaseUri = "http://image.tmdb.org/t/p/w500";
        private const string DefaultActorImage = "https://ih1.redbubble.net/image.11241105.4427/ra%2Cunisex_tshirt%2Cx925%2C5e504c%3A7bf03840f4%2Cfront-c%2C217%2C190%2C210%2C230-bg%2Cf8f8f8.lite-1.jpg";
        private const string ImdbNameBaseUrl = "https://www.imdb.com/name/";
        private const string DefaultTrailer = "https://www.youtube.com/embed/fM9wnGIlDsc";

        private string[] DateTimeFormats = new string[] { "yyyy-dd-MM", "yyyy-MM-dd", };

        public JsonProvider()
        {
        }

        public IEnumerable<Movie> ExtractFoundMoviesFromSearchMovieJsonResult(string jsonAsString)
        {
            if (jsonAsString == "ERROR")
            {
                throw new InvalidOperationException("Cannot search with empty title");
            }

            var moviesAsJson = JObject.Parse(jsonAsString);

            var foundMovies = new List<Movie>();

            var results = moviesAsJson["results"];

            foreach (var m in results)
            {
                var currentMovie = new Movie
                {
                    ApiMovieId = int.Parse(m["id"].ToString()),
                    Title = m["title"].ToString(),
                    Description = m["overview"].ToString()
                };

                foundMovies.Add(currentMovie);
            }

            return foundMovies;
        }

        public IEnumerable<Genre> ExtractGenresFromListAllGenresJsonResult(string jsonAsString)
        {
            var json = JObject.Parse(jsonAsString);

            var genres = json["genres"];

            var existingGenres = new List<Genre>();

            foreach (var genre in genres)
            {
                existingGenres.Add(new Genre
                {
                    ApiGenreId = int.Parse(genre["id"].ToString()),
                    Name = genre["name"].ToString()
                });
            }

            return existingGenres;
        }

        public Movie ExtractMovieFromMovieDetailsJsonResult(string jsonAsString)
        {
            var json = JObject.Parse(jsonAsString);

            // handle not found resource at TheMovieDB
            if (json.ContainsKey("status_code"))
                if (json["status_code"].ToString() == "34")
                    throw new ResourceNotFoundException(nameof(Movie));


            var apiMovieId = int.Parse(json["id"].ToString());
            var title = json["title"].ToString();
            var description = json["overview"].ToString();

            var durationStr = json["runtime"].ToString();
            int? durationInMinutes = int.TryParse(durationStr, out var tempVal) ? tempVal : (int?)null;

            var rating = float.Parse(json["vote_average"].ToString());
            var releaseDateString = json["release_date"].ToString();

            DateTime releaseDate = new DateTime();
            bool isDateValid = DateTime.TryParseExact(
                    releaseDateString,
                    DateTimeFormats,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out releaseDate);

            releaseDate = isDateValid ? releaseDate : new DateTime(0001, 1, 1);

            var posterPath = json["poster_path"].ToString();

            var trailerUrl = string.Empty;
            var videos = json["videos"]["results"];

            if (videos.Any())
            {
                foreach (var video in videos)
                {
                    if (trailerUrl != "")
                    {
                        break;
                    }

                    var type = video["type"].ToString();
                    var site = video["site"].ToString();
                    var videoKey = video["key"].ToString();

                    if (type == "Trailer" && site == "YouTube")
                    {
                        trailerUrl = YouTubeBaseUri + videoKey;
                    }
                }
            }
            else
            {
                trailerUrl = DefaultTrailer;
            }

            var movie = new Movie
            {
                ApiMovieId = apiMovieId,
                Title = title,
                Description = description,
                DurationInMinutes = durationInMinutes,
                Rating = rating,
                ReleaseDate = releaseDate,
                SmallImageUrl = SmallImageBaseUri + posterPath,
                MediumImageUrl = MediumImageBaseUri + posterPath,
                LargeImageUrl = LargeImageBaseUri + posterPath,
                TrailerUrl = trailerUrl
            };

            return movie;
        }

        public IEnumerable<(int GenreApiId, string GenreName)> ExtractGenresForMovie(string jsonAsString)
        {
            var json = JObject.Parse(jsonAsString);

            // handle not found resource at TheMovieDB
            if (json.ContainsKey("status_code") && json["status_code"].ToString() == "34")
            {
                throw new ResourceNotFoundException(nameof(Movie));
            }

            var genres = json["genres"];
            var movieApiGenresIds = new List<(int, string)>();

            foreach (var genre in genres)
            {
                int apiGenreId = int.Parse(genre["id"].ToString());
                string genreName = genre["name"].ToString();

                movieApiGenresIds.Add((apiGenreId, genreName));
            }

            return movieApiGenresIds;
        }

        public IEnumerable<(Actor Actor, string MovieCharacter)> ExtractActorsFromMovieCastJsonResult(string jsonAsString)
        {
            var json = JObject.Parse(jsonAsString);

            var actors = json["cast"];

            var actorsCast = new List<(Actor, string)>();

            foreach (var actor in actors.Take(5))
            {
                var imagePath = actor["profile_path"].ToString();

                var movieActor = new Actor
                {
                    ApiActorId = int.Parse(actor["id"].ToString()),
                    FullName = actor["name"].ToString(),
                    SmallImageUrl = imagePath == "" ? DefaultActorImage : SmallImageBaseUri + imagePath,
                    MediumImageUrl = imagePath == "" ? DefaultActorImage : MediumImageBaseUri + imagePath,
                    LargeImageUrl = imagePath == "" ? DefaultActorImage : LargeImageBaseUri + imagePath
                };

                var movieCharacter = actor["character"].ToString();

                actorsCast.Add((movieActor, movieCharacter));
            }

            return actorsCast;
        }

        public Actor ExtractActorDetails(string actorDetailsJsonResult)
        {
            var json = JObject.Parse(actorDetailsJsonResult);
            var imdbKey = json["imdb_id"].ToString();

            var actorDTO = new Actor
            {
                DateOfBirth = json["birthday"].ToString(),
                PlaceOfBirth = json["place_of_birth"].ToString(),
                Biography = json["biography"].ToString(),
                ImdbId = imdbKey,
                ImdbProfileUrl = ImdbNameBaseUrl + imdbKey
            };

            return actorDTO;
        }
    }
}
