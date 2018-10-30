using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using TelFlix.Data.Contracts;
using TelFlix.Data.Models;
using TelFlix.Data.UnitOfWorkCore;
using TelFlix.Services.Abstract;
using TelFlix.Services.Contracts;

namespace TelFlix.Services
{
    public class CreateMovieService : ICreateMovieService// BaseService,
    {
        private const string JsonSplitString = "/";

        // JSON properties
        private const string PropertyTitle = "Title";
        private const string PropertyReleased = "Released";
        private const string PropertyRuntime = "Runtime";
        private const string PropertyRatings = "Ratings";
        private const string PropertyValue = "Value";
        private const string PropertyPlot = "Plot";
        private const string DateFormatString = "d MMM yyyy";

        //public CreateMovieService(IUnitOfWork unitOfWork, UserManager<User> userManager)
        //    : base(unitOfWork, userManager)
        //{
        //}
        private readonly IMovieRepository movieRepository;

        public CreateMovieService(IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository;
        }

        public (string Title, Movie Movie) CreateMovie(JObject movieJson)
        {
            DateTime releaseDate = new DateTime();

            var title = movieJson[PropertyTitle]
                .ToString();

            var isDateValid = DateTime
                .TryParseExact(
                    movieJson[PropertyReleased]
                        .ToString(),
                    DateFormatString,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out releaseDate);

            releaseDate = isDateValid ? releaseDate : new DateTime(0001, 1, 1);

            var durationInMinutes = int
                .Parse(movieJson[PropertyRuntime]
                .ToString()
                .Split()[0]);

            var rating = float
                .Parse(movieJson[PropertyRatings][0][PropertyValue]
                .ToString()
                .Split(JsonSplitString)[0]);

            var description = movieJson[PropertyPlot]
                .ToString();

            Movie movie = new Movie
            {
                Title = title,
                ReleaseDate = releaseDate,
                DurationInMinutes = durationInMinutes,
                Rating = rating,
                Description = description,
            };


            return (title, movie);
        }
    }
}
