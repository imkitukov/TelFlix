using System.Collections.Generic;
using TelFlix.Data.Models;
using TelFlix.Services.Models.Messages;

namespace TelFlix.Services.Contracts
{
    public interface IMessageServices
    {
        IEnumerable<MessageViewModel> ListReceivedMessages(string id, int page = 1, int pageSize = 10);

        IEnumerable<MessageViewModel> ListSentMessages(string id, int page = 1, int pagesize = 10);

        IEnumerable<MessageViewModel> GetWishlistRequests(string id);

        Message AddMessage(Message message);

        Message DeleteMessage(int id);
    }
}
