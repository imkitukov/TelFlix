using System;

namespace TelFlix.Services.Providers.Exceptions
{
    public class PasswordsDoesntMatchException : Exception
    {
        private const string PasswordsDoesntMatchExcMessage = "Passwords doesnt match, please try to register again!";

        public PasswordsDoesntMatchException()
            : base(PasswordsDoesntMatchExcMessage)
        {
        }
    }
}
