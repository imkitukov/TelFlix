using System;
using TelFlix.Data.Models.Contracts;

namespace TelFlix.Data.Models
{
    public class MoviesDirectors : IDeletable
    {
        public int MovieId { get; set; }

        public int DirectorId { get; set; }

        public Movie Movie { get; set; }

        public Director Director { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}