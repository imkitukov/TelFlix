using System.ComponentModel.DataAnnotations;

namespace TelFlix.App.Areas.Moderator.Models.Movie
{
    public class EditMovieFormModel
    {
        [Required]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 500 symbols!")]
        public string Title { get; set; }

        [Required]
        [Range(0, 1000, ErrorMessage = "Duration must be positive number, 1000 minutes maximum")]
        [Display(Name = "Duration in minutes")]
        public int? DurationInMinutes { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 1000 symbols!")]
        public string Description { get; set; }

        [Required]
        [Url]
        [Display(Name = "Trailer Url")]
        public string TrailerUrl { get; set; }

        public GenreCheckBox[] Genres { get; set; }
    }

    public class GenreCheckBox
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }
}
