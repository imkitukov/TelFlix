using System;

namespace TelFlix.Services.Providers.Exceptions
{
    public class InexistingEntityException : Exception
    {
        private const string ExceptionMessage = "{0} : <{1}> not found in our database!";

        public InexistingEntityException(string entity, string searchedEntity)
            : base(string.Format(ExceptionMessage, entity, searchedEntity))
        {
        }
    }
}
