using System;

namespace TelFlix.Services.Providers.Exceptions
{
    public class InvalidCredentialsException : Exception
    {
        private const string InvalidCredentialsExcMessage = "Invalid credentials, please try to login again!";

        public InvalidCredentialsException()
            : base(InvalidCredentialsExcMessage)
        {
        }
    }
}
