using System;
using TelFlix.Data.Models.Contracts;

namespace TelFlix.Data.Models
{
    public class MoviesUsers : IDeletable
    {
        public int MovieId { get; set; }

        public string UserId { get; set; }

        public Movie Movie { get; set; }

        public User User { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}