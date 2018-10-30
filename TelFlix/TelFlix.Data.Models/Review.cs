using System.ComponentModel.DataAnnotations;
using TelFlix.Data.Models.Abstract;

namespace TelFlix.Data.Models
{
    public class Review : BaseEntity
    {
        [MaxLength(500)]
        public string Comment { get; set; }

        public int MovieId { get; set; }

        public Movie Movie { get; set; }

        public string UserId { get; set; }

        public User Author { get; set; }
    }
}