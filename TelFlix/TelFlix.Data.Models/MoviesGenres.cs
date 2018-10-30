using System;
using TelFlix.Data.Models.Contracts;

namespace TelFlix.Data.Models
{
    public class MoviesGenres : IDeletable
    {
        public int MovieId { get; set; }

        public int GenreId { get; set; }

        public Movie Movie { get; set; }

        public Genre Genre { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}