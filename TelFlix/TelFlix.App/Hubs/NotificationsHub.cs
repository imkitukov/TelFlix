﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
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

        public static Dictionary<string, string> ConnectedUsers { get; set; } = new Dictionary<string, string>();

        public NotificationsHub(UserManager<User> userManager, IMessageServices messageServices)
        {
            this.userManager = userManager;
            this.messageServices = messageServices;
        }

        public async Task SendMessage(string receiver, string subject, string content)
        {
            //var admins = await this.userManager.GetUsersInRoleAsync("Administrator");
            var sender = this.Context.User.Identity.Name;
            var senderUser = await this.userManager.FindByEmailAsync(sender);
            var receiverUser = await this.userManager.FindByEmailAsync(receiver);

            var message = new Message
            {
                Sender = senderUser,
                Receiver = receiverUser,
                Subject = subject,
                Content = content
            };

            this.messageServices.AddMessage(message);

            if (ConnectedUsers.ContainsKey(receiver))
            {
                await this.Clients
                    .Client(ConnectedUsers[receiver])
                    .SendAsync("pushNotification");
            }

            //await this.Clients.Groups("Admins").SendAsync("getMessage", sender, content);
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
