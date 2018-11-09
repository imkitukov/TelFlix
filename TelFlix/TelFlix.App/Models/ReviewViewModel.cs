using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TelFlix.Data.Models;

namespace TelFlix.App.Models
{
    public class ReviewViewModel
    {
        [Required(ErrorMessage = "Please provide a review")]
        [MaxLength(500)]
        public string Comment { get; set; }

        public int MovieId { get; set; }

        public User Author { get; set; }
    }
}
