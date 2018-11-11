using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelFlix.Data.Models;
using TelFlix.Services.Contracts;
using TelFlix.Services.Models.Messages;

namespace TelFlix.App.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IMessageServices messageService;

        public UsersController(UserManager<User> userManager, IMessageServices messageService)
        {
            this.userManager = userManager;
            this.messageService = messageService;
        }

        [Authorize]
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
                ViewData["type"] = "Sent";
                messages = this.messageService
                    .GetWishlistRequests(user.Id)
                    .ToList();
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
    }
}