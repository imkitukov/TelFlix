namespace TelFlix.Services.Models.Movie
{
    public class TopListMovieModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int? DurationInMinutes { get; set; }

        public float? Rating { get; set; }

        public string SmallImageUrl { get; set; }

        public string MediumImageUrl { get; set; }

        public string TrailerUrl { get; set; }
    }
}
