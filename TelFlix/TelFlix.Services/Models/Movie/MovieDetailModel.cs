using System;
using System.Collections.Generic;
using TelFlix.Data.Models;
using TelFlix.Services.Models.Reviews;

namespace TelFlix.Services.Models.Movie
{
    public class MovieDetailModel
    {
        public int Id { get; set; }

        public int ApiMovieId { get; set; }

        public string Title { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public int? DurationInMinutes { get; set; }

        public IEnumerable<Actor> Actors { get; set; }

        public IEnumerable<Genre> Genres { get; set; }

        public IEnumerable<ReviewModel> Reviews { get; set; }

        public float? Rating { get; set; }

        public string Description { get; set; }

        public string SmallImageUrl { get; set; }

        public string MediumImageUrl { get; set; }

        public string TrailerUrl { get; set; }

        public bool IsInLibrary { get; set; }
    }
}
