using System;

namespace TelFlix.Services.Providers.Exceptions
{
    public class AuthorizationRequiredException : Exception
    {
        private const string NotPermitedExcMessage = "You dont have permission to use this command {0}!";

        public AuthorizationRequiredException()
        {
        }

        public AuthorizationRequiredException(string commandName)
           : base(string.Format(NotPermitedExcMessage, commandName))
        {
        }
    }
}
