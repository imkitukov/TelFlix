using System;

namespace TelFlix.Services.Providers.Exceptions
{
    public class EntityAlreadyExistingException : Exception
    {
        private const string ExcMessage = "{0} <{1}> already exist in {2}!";

        public EntityAlreadyExistingException(string entityType, string entityToAdd, string whereExists = "")
            : base(string.Format(ExcMessage, entityType, entityToAdd, whereExists))
        {
        }
    }
}
