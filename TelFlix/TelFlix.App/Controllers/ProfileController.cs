using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Security.Claims;
using TelFlix.App.Models.Users;
using TelFlix.Services.Contracts;

namespace TelFlix.App.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IReviewService reviewServices;
        private readonly IFavouritesService favouritesService;
        private readonly IUserServices userServices;

        public ProfileController(IReviewService reviewServices, IFavouritesService favouritesService, IUserServices userServices)
        {
            this.reviewServices = reviewServices;
            this.favouritesService = favouritesService;
            this.userServices = userServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Account(string returnUrl = "")
        {
            if (returnUrl == string.Empty)
            {
                var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var balance = this.userServices.GetAccountBalanance(userId);

                var creditCards = this.userServices
                    .GetCreditCardsByUserId(userId)
                    .ToList();

                var model = new UserAccountViewModel
                {
                    Balance = balance,
                    CreditCardsListItems = new SelectList(creditCards, "Id", "Number"),
                    CreditCards = creditCards
                };

                return View(model);
            }
            else
            {
                return Redirect(returnUrl);
            }
        }

        [HttpPost]
        public IActionResult FundAccount(decimal amount)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            this.userServices.FundAccount(userId, amount);

            return RedirectToAction(nameof(Account), "Profile");
        }

        public IActionResult Library()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var mm = this.favouritesService.GetAllFavoritesByUserId(userId);
            return View(mm);
        }

        public IActionResult WishList()
        {
            return View();
        }

        public IActionResult Reviews()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var rm = this.reviewServices.GetAllByUserId(userId);
            return View(rm);
        }
    }
}