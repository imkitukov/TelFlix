using System;

namespace TelFlix.Services.Providers.Exceptions
{
    public class InvalidParamsCountException : Exception
    {
        private const string InvalidParamsCount = "Invalid number of parameters! Please try again with params: {0}";

        public InvalidParamsCountException()
        {
        }

        public InvalidParamsCountException(string correctParams)
           : base(string.Format(InvalidParamsCount, correctParams))
        {
        }
    }
}
