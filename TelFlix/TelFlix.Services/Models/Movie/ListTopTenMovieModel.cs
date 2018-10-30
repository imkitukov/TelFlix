using System;

namespace TelFlix.Services.ViewModels.MovieViewModels
{
    public class ListTopTenMovieModel
    {
        public string Title { get; set; }

        public float? Rating { get; set; }

        public DateTime? ReleaseDate { get; set; }
    }
}
