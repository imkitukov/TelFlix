using System;

namespace TelFlix.Services.Models.Reviews
{
    public class ReviewModel
    {
        public int Id { get; set; }

        public string Comment { get; set; }

        public string Author { get; set; }

        public string AuthorId { get; set; }

        public string MovieTitle { get; set; }

        public DateTime? CreatedOn { get; set; }
    }
}
