using System;

namespace TelFlix.Services.Providers.Exceptions
{
    public class UsernameAlreadyExistsException : Exception
    {
        private const string UsernameAlreadyExistsExcMessage = "Username <{0}> already existing, please choose another one!";

        public UsernameAlreadyExistsException(string username)
            : base(string.Format(UsernameAlreadyExistsExcMessage, username))
        {
        }
    }
}
