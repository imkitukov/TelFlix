using TelFlix.Data.Models.Contracts;
using TelFlix.Data.Repository;

namespace TelFlix.Data.UnitOfWorkCore
{
    public interface IUnitOfWork
    {
        IRepository<T> GetRepo<T>() where T : class, IDeletable;

        int SaveChanges();
    }
}
