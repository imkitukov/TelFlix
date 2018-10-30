using System;

namespace TelFlix.Services.Providers.Exceptions
{
    public class NotLoggedInException : Exception
    {
        private const string NotLoggedInMessage = "Please login or register first.";

        public NotLoggedInException()
            : base(NotLoggedInMessage)
        {
        }
    }
}
