using System;

namespace TelFlix.Services.Providers.Exceptions
{
    public class InvalidPropertyToEditException : Exception
    {
        private const string ExcMessage = "Invalid property to edit, please try again command in format! {0}";

        public InvalidPropertyToEditException(string correctParams)
            : base(string.Format(ExcMessage, correctParams))
        {
        }
    }
}
