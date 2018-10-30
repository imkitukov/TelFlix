using System;

namespace TelFlix.Services.Providers.Exceptions
{
    public class StringCannotBeNullOrWhiteSpaceException : Exception
    {
        private const string ExceptionMessage = "{0} cannot be null or whitespace!";

        public StringCannotBeNullOrWhiteSpaceException(string param)
            : base(string.Format(ExceptionMessage, param))
        {
        }
    }
}
