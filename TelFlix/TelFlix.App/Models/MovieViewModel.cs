using TelFlix.Data.Models;

namespace TelFlix.App.Models
{
    public class MovieViewModel
    {
        public MovieViewModel(Movie movie)
        {
            this.Id = movie.Id;
            this.ApiMovieId = movie.ApiMovieId;
            this.Title = movie.Title;
        }

        public int Id { get; set; }

        public int ApiMovieId { get; set; }

        public string Title { get; set; }

        public int MyProperty { get; set; }
    }
}
