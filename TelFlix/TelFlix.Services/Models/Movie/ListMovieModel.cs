using System;
using System.Collections.Generic;
using TelFlix.Data.Models;

namespace TelFlix.Services.Models.Movie
{
    public class ListMovieModel
    {
        public int Id { get; set; }

        public int ApiMovieId { get; set; }

        public string Title { get; set; }

        public float? Rating { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public int? Duration { get; set; }

        public string SmallImageUrl { get; set; }

        public IList<Genre> Genres { get; set; }
    }
}
