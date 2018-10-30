using System;
using System.Collections.Generic;
using TelFlix.Data.Context;
using TelFlix.Data.Models.Contracts;
using TelFlix.Data.Repository;

namespace TelFlix.Data.UnitOfWorkCore
{
    public class UnitOfWork : IUnitOfWork
    {
        private ITFContext context;
        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public UnitOfWork(ITFContext context)
        {
            this.context = context;
        }

        public IRepository<T> GetRepo<T>() where T : class, IDeletable
        {
            var repoType = typeof(Repository<T>);

            if (!repositories.ContainsKey(repoType))
            {
                var repo = Activator.CreateInstance(repoType, this.context);
                repositories[repoType] = repo;
            }

            return (IRepository<T>)repositories[repoType];
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }
    }
}
