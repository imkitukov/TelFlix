using System.Collections.Generic;
using TelFlix.Services.Models;

namespace TelFlix.Services.Contracts
{
    public interface IUserServices
    {
        decimal GetAccountBalanance(string userId);

        void ChargeAccount(string userId, decimal amount);

        void FundAccount(string userId, decimal amount);

        IEnumerable<CreditCardModel> GetCreditCardsByUserId(string userId);
    }
}
