using System.ComponentModel.DataAnnotations;

namespace TelFlix.App.Models
{
    public class ReviewViewModel
    {
        [Required(ErrorMessage = "Please provide a content")]
        [MaxLength(500)]
        public string Comment { get; set; }

        public int MovieId { get; set; }
    }
}
