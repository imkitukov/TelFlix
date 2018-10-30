using System.Net;

namespace TelFlix.Services.Contracts
{
    public interface IWebClient
    {
        WebClient Client { get; set; }
    }
}
