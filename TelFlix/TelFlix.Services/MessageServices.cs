using System;
using System.Collections.Generic;
using System.Linq;
using TelFlix.Data.Context;
using TelFlix.Data.Models;
using TelFlix.Services.Abstract;
using TelFlix.Services.Contracts;
using TelFlix.Services.Models.Messages;

namespace TelFlix.Services
{
    public class MessageServices : BaseService, IMessageServices
    {
        public MessageServices(TFContext context)
            : base(context)
        {
        }

        public Message AddMessage(Message message)
        {
            this.Context
                .Messages
                .Add(message);

            this.Context
                .SaveChanges();

            return message;
        }

        public Message DeleteMessage(int id)
        {
            var messageToDelete = this.Context
                .Messages
                .Find(id);

            this.Context
                .Messages
                .Remove(messageToDelete);

            this.Context
                .SaveChanges();

            return messageToDelete;
        }

        public IEnumerable<MessageViewModel> GetWishlistRequests(string id)
             => this.Context
                    .Messages
                    .Where(m => m.ReceiverId == id && m.Subject == "Add movie to db")
                    .Select(m => new MessageViewModel
                    {
                        Id = m.Id,
                        Receiver = m.Receiver,
                        Sender = m.Sender,
                        Subject = m.Subject,
                        Content = m.Content,
                        CreatedOn = (DateTime)m.CreatedOn
                    })
                    .OrderByDescending(m => m.CreatedOn)
                    .ToList();

        public IEnumerable<MessageViewModel> ListReceivedMessages(string id, int page = 1, int pageSize = 10)
            => this.Context
                .Messages
                .Where(m => m.ReceiverId == id && m.Subject != "Add movie to db")
                .Select(m => new MessageViewModel
                {
                    Id = m.Id,
                    Receiver = m.Receiver,
                    Sender = m.Sender,
                    Subject = m.Subject,
                    Content = m.Content,
                    CreatedOn = (DateTime)m.CreatedOn
                })
                .OrderByDescending(m => m.CreatedOn)
                .ToList();

        public IEnumerable<MessageViewModel> ListSentMessages(string id, int page = 1, int pagesize = 10)
            => this.Context
                .Messages
                .Where(m => m.SenderId == id)
                .Select(m => new MessageViewModel
                {
                    Id = m.Id,
                    Receiver = m.Receiver,
                    Sender = m.Sender,
                    Subject = m.Subject,
                    Content = m.Content,
                    CreatedOn = (DateTime)m.CreatedOn
                })
                .OrderByDescending(m => m.CreatedOn)
                .ToList();
    }
}
