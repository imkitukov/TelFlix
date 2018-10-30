using System;

namespace TelFlix.Services.Providers.Exceptions
{
    public class InvalidDateFormatException : Exception
    {
        private const string InvalidDateFormatExcMessage = "Invalid date format, please try again!";

        public InvalidDateFormatException()
            : base(InvalidDateFormatExcMessage)
        {
        }
    }
}
