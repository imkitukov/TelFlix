using System;

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
    }
}
