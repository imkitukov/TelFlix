using System;
using TelFlix.Data.Models;

namespace TelFlix.Services.Models.Messages
{
    public class MessageViewModel
    {
        public int Id { get; set; }

        public User Sender { get; set; }

        public User Receiver { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsMovieAddedToDb { get; set; } = false;
    }
}
