using System;
using System.Linq;
using System.Linq.Expressions;
using TelFlix.Data.Models.Contracts;

namespace TelFlix.Data.Repository
{
    public interface IRepository<T> where T : class, IDeletable
    {
        IQueryable<T> GetAll();

        IQueryable<T> GetAll(Expression<Func<T, bool>> filter);

        IQueryable<T> GetAllAndDeleted();

        T GetFirstOrDefault(Expression<Func<T, bool>> filter);

        //IEnumerable<T> Find(Func<T, bool> filter);

        T GetById(int id);

        void Add(T entity);

        void Delete(T entity);

        void Update(T entity);

        int Count(Func<T, bool> predicate);

        bool IsSeeded();

        int SaveChanges();
    }
}
