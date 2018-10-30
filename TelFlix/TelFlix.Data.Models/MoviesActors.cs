using System;
using TelFlix.Data.Models.Contracts;

namespace TelFlix.Data.Models
{
    public class MoviesActors : IDeletable
    {
        public int MovieId { get; set; }

        public int ActorId { get; set; }

        public Movie Movie { get; set; }

        public Actor Actor { get; set; }

        public string MovieCharacter { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}