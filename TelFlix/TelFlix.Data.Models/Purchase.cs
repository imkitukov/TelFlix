using TelFlix.Data.Models.Abstract;

namespace TelFlix.Data.Models
{
    public class Purchase : BaseEntity
    {
        public decimal Amount { get; set; }

        public PurchaseType Type { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}
