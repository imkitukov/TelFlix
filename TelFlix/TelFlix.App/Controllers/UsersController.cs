using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelFlix.Data.Models;
using TelFlix.Services.Contracts;
using TelFlix.Services.Models.Messages;

namespace TelFlix.App.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IMessageServices messageService;
        private readonly IUserServices userServices;
        private readonly IFavouritesService favouritesService;
        private readonly IMovieServices movieServices;

        public UsersController(UserManager<User> userManager, IMessageServices messageService, IUserServices userServices, IFavouritesService favouritesService, IMovieServices movieServices)
        {
            this.userManager = userManager;
            this.messageService = messageService;
            this.userServices = userServices;
            this.favouritesService = favouritesService;
            this.movieServices = movieServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Inbox(string type)
        {
            var user = await this.userManager
                .FindByEmailAsync(this.User.Identity.Name);

            List<MessageViewModel> messages;

            if (type == "received")
            {
                ViewData["type"] = "Received";
                messages = this.messageService
                    .ListReceivedMessages(user.Id)
                    .ToList();
            }
            else if (type == "wishlist")
            {
                ViewData["type"] = "Received";
                messages = this.messageService
                    .GetWishlistRequests(user.Id)
                    .ToList();

                // send only message for not added movies yet
                foreach (var message in messages)
                {
                    int movieApiId = int.Parse(message.Content);
                    bool movieCreated = this.movieServices.ApiIdExists(movieApiId);
                    message.IsMovieAddedToDb = movieCreated;
                }

                messages = messages.Where(m => m.IsMovieAddedToDb == false).ToList();
            }
            else
            {
                ViewData["type"] = "Sent";
                messages = this.messageService
                    .ListSentMessages(user.Id)
                    .ToList();
            }

            return View(messages);
        }

        [HttpPost]
        public IActionResult DeleteMessage(int id)
        {
            this.messageService.DeleteMessage(id);

            return NoContent();
        }

        public IActionResult Watch(int movieId, string type)
        {
            var userName = this.User.Identity.Name;
            var userId = this.userManager.FindByEmailAsync(userName).Result.Id;

            // Check user's account balance
            var accountBalance = this.userServices.GetAccountBalanance(userId);

            if (type == "rent")
            {
                if (accountBalance < 1.95m)
                {
                    // Partial view with a link to fund account
                    return RedirectToAction("Account", "Profile", new { returnUrl = $"/Movies/Details/{movieId}" });
                }

                try
                {
                    this.userServices.ChargeAccount(userId, 1.95m);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                if (accountBalance < 19.95m)
                {
                    // Partial view with a link to fund account
                    return RedirectToAction("Account", "Profile");
                }

                try
                {
                    this.userServices.ChargeAccount(userId, 19.95m);
                    this.favouritesService.AddMovieToFavourite(movieId, userId);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            // Partial view with the modal
            return RedirectToAction("Details", "Movies", new { id = movieId });
        }
    }
}