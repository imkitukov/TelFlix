using System.ComponentModel.DataAnnotations;
using TelFlix.Data.Models.Abstract;

namespace TelFlix.Data.Models
{
    public class CreditCard : BaseEntity
    {
        [CreditCard]
        public int Number { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}
