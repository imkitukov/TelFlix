using TelFlix.Data.Models.Abstract;

namespace TelFlix.Data.Models
{
    public class Message : BaseEntity
    {
        public bool IsRead { get; set; } = false;

        public string Subject { get; set; }

        public string Content { get; set; }

        public string ReceiverId { get; set; }

        public User Receiver { get; set; }

        public string SenderId { get; set; }

        public User Sender { get; set; }
    }
}
