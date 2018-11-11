using System.Collections.Generic;
using System.Linq;
using TelFlix.Data.Context;
using TelFlix.Data.Models;
using TelFlix.Services.Abstract;
using TelFlix.Services.Contracts;
using TelFlix.Services.Models;
using TelFlix.Services.Providers.Exceptions;

namespace TelFlix.Services
{
    public class UserServices : BaseService, IUserServices
    {
        public UserServices(TFContext context)
            : base(context)
        {
        }

        public IEnumerable<CreditCardModel> GetCreditCardsByUserId(string userId)
            => this.Context
                .CreditCards
                .Where(cc => cc.UserId == userId)
                .Select(cc => new CreditCardModel
                {
                    Id = cc.Id,
                    Number = cc.Number,
                    CreatedOn = cc.CreatedOn
                })
                .ToList();

        public decimal GetAccountBalanance(string userId)
        {
            var user = this.Context.Users.Find(userId);

            if (user == null)
            {
                throw new InexistingEntityException(nameof(User), userId);
            }

            return user.AccountBalance;
        }

        public void ChargeAccount(string userId, decimal amount)
        {
            var user = this.Context.Users.Find(userId);

            if (user == null)
            {
                throw new InexistingEntityException(nameof(User), userId);
            }

            user.AccountBalance -= amount;
            this.Context.SaveChanges();
        }

        public void FundAccount(string userId, decimal amount)
        {
            var user = this.Context.Users.Find(userId);

            if (user == null)
            {
                throw new InexistingEntityException(nameof(User), userId);
            }

            user.AccountBalance += amount;
            this.Context.SaveChanges();
        }
    }
}
