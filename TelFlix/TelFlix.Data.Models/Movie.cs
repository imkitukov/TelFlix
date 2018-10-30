using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TelFlix.Data.Models.Abstract;

namespace TelFlix.Data.Models
{
    public class Movie : BaseEntity
    {
        [Required]
        public int ApiMovieId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Title { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public int? DurationInMinutes { get; set; }

        public ICollection<MoviesActors> MoviesActors { get; set; }

        public ICollection<MoviesGenres> MoviesGenres { get; set; }

        public ICollection<MoviesDirectors> Directors { get; set; }

        public ICollection<MoviesUsers> MoviesUsers { get; set; }

        public ICollection<Review> Reviews { get; set; }

        public float? Rating { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public string SmallImageUrl { get; set; }

        public string MediumImageUrl { get; set; }

        public string LargeImageUrl { get; set; }

        public string TrailerUrl { get; set; }
    }
}
