using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TelFlix.Data.Models.Abstract;

namespace TelFlix.Data.Models
{
    public class Genre : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        
        [Required]
        public int ApiGenreId { get; set; }

        public ICollection<MoviesGenres> MoviesGenres { get; set; }
    }
}