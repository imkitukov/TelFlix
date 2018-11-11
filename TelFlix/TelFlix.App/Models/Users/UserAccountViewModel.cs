using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using TelFlix.Data.Models;
using TelFlix.Services.Models;

namespace TelFlix.App.Models.Users
{
    public class UserAccountViewModel
    {
        public string UserId { get; set; }

        public decimal Balance { get; set; }

        public SelectList CreditCardsListItems { get; set; }

        public IEnumerable<CreditCardModel> CreditCards { get; set; }

        public int CreditCardId { get; set; }
    }
}
