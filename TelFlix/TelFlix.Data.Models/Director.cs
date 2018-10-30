using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TelFlix.Data.Models.Abstract;

namespace TelFlix.Data.Models
{
    public class Director : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public ICollection<MoviesDirectors> Movies { get; set; }
    }
}