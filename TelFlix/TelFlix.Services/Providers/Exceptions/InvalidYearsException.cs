using System;

namespace TelFlix.Services.Providers.Exceptions
{
    public class InvalidYearsException : Exception
    {
        private const string ExceptionMessage = "Invalid years input! Please try again with params: {0}";

        public InvalidYearsException(string correctCommandParams)
            : base(string.Format(ExceptionMessage, correctCommandParams))
        {
        }
    }
}
