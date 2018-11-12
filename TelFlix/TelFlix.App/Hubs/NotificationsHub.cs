using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TelFlix.Data.Models;
using TelFlix.Services.Contracts;

namespace TelFlix.App.Hubs
{
    [Authorize]
    public class NotificationsHub : Hub
    {
        private readonly UserManager<User> userManager;
        private readonly IMessageServices messageServices;
        private readonly IMovieServices movieServices;

        public static Dictionary<string, string> ConnectedUsers { get; set; } = new Dictionary<string, string>();

        public NotificationsHub(UserManager<User> userManager, IMessageServices messageServices, IMovieServices movieServices)
        {
            this.userManager = userManager;
            this.messageServices = messageServices;
            this.movieServices = movieServices;
        }

        public async Task SendMessage(string receiver, string subject, string content)
        {
            //var admins = await this.userManager.GetUsersInRoleAsync("Administrator");
            var sender = this.Context.User.Identity.Name;
            var senderUser = await this.userManager.FindByEmailAsync(sender);
            var receiverUser = await this.userManager.FindByEmailAsync(receiver);

            if (receiver == "Moderators" && subject == "Add movie to db")
            {
                var moderators = await this.userManager.GetUsersInRoleAsync("Moderator");

                foreach (var mod in moderators)
                {
                    var modMessage = new Message
                    {
                        Sender = senderUser,
                        Receiver = mod,
                        Subject = subject,
                        Content = content
                    };

                    this.messageServices.AddMessage(modMessage);
                }
            }
            // send to concrete user/receiver 
            else
            {
                var message = new Message
                {
                    Sender = senderUser,
                    Receiver = receiverUser,
                    Subject = subject,
                    Content = content
                };

                if (subject == "Added to wishlist")
                {
                    int addedMovieApiId = int.Parse(message.Content);

                    // trying to get movie before actually movie is added to db :/
                    //var movie = await this.movieServices.GetMovieByApiId(addedMovieApiId);
                    // TODO add url /movie/details/{movie.Id} to message!
                    //string successfullyAddedMovieRequestMessageContent = $"Hey {receiver}! Movie with title <{movie.Title}> successfully added to TelFlix thanks to your request! Enjoy our site :)";

                    string successfullyAddedMovieRequestMessageContent = $"Hey {receiver}! Your movie request successfully added to TelFlix! Enjoy our site :)";

                    message.Subject = "Wishlist request satisfied";
                    message.Content = successfullyAddedMovieRequestMessageContent;
                }

                this.messageServices.AddMessage(message);

                if (ConnectedUsers.ContainsKey(receiver))
                {
                    await this.Clients
                        .Client(ConnectedUsers[receiver])
                        .SendAsync("pushNotification");
                }
            }

            await this.Clients.Groups("Moderators").SendAsync("pushNotification", sender, content);
        }

        public override async Task OnConnectedAsync()
        {
            var currentUser = this.Context.User;
            var connedtionId = this.Context.ConnectionId;

            if (!ConnectedUsers.ContainsKey(currentUser.Identity.Name))
            {
                ConnectedUsers.Add(currentUser.Identity.Name, connedtionId);
            }

            ConnectedUsers[currentUser.Identity.Name] = connedtionId;

            if (currentUser.IsInRole("Administrator"))
            {
                await this.Groups.AddToGroupAsync(this.Context.ConnectionId, "Admins");
                await this.Groups.AddToGroupAsync(this.Context.ConnectionId, "ElevatedUsers");
            }
            else if (currentUser.IsInRole("Moderator"))
            {
                await this.Groups.AddToGroupAsync(this.Context.ConnectionId, "Moderators");
                await this.Groups.AddToGroupAsync(this.Context.ConnectionId, "ElevatedUsers");
            }
            else
            {
                await this.Groups.AddToGroupAsync(this.Context.ConnectionId, "Regulars");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var currentUser = this.Context.User;

            if (ConnectedUsers.ContainsKey(currentUser.Identity.Name))
            {
                string garbage;
                ConnectedUsers.Remove(currentUser.Identity.Name, out garbage);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
