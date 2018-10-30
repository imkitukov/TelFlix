using System;
using System.Collections.Generic;
using System.Text;
using TelFlix.Data.Models.Contracts;

namespace TelFlix.Services.Contracts
{
    public interface ISeedDatabaseService
    {
        bool Check<T>() where T : class, IDeletable;

        void SeedAsync();
    }
}
