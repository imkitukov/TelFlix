using System;

namespace TelFlix.Services.Models
{
    public class CreditCardModel
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public DateTime? CreatedOn { get; set; }
    }
}
