using System;

namespace TelFlix.Services.Providers.Exceptions
{
    public class ResourceNotFoundException : Exception
    {
        private const string ExcMessage = "The <{0}> you requested could not be found!";

        public ResourceNotFoundException(string entityType)
          : base(string.Format(ExcMessage, entityType))
        {
        }
    }
}
