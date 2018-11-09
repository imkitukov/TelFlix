using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;
using TelFlix.Data.Models;

namespace TelFlix.App.Hubs
{
    public class NotificationsHub : Hub
    {
        private readonly UserManager<User> userManager;

        public NotificationsHub(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task SendMessage(string username, string content)
        {
            // Send a message to the admins' Inboxes

            //var admins = await this.userManager.GetUsersInRoleAsync("Administrator");

            await this.Clients.All.SendAsync("getMessage", username, content);
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }
    }
}
