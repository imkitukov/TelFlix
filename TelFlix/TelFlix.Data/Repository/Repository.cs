using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TelFlix.Data.Context;
using TelFlix.Data.Models.Contracts;

namespace TelFlix.Data.Repository
{
    public class Repository<T> : IRepository<T>
        where T : class, IDeletable
    {
        //protected readonly ITFContext context;

        //private DbSet<T> entities;

        public Repository(ITFContext context)
        {
            this.Context = context;
            this.Entites = context.Set<T>();
        }

        protected ITFContext Context { get; set; }

        protected DbSet<T> Entites { get; set; }


        public int SaveChanges() => this.Context.SaveChanges();

        public void Add(T entity)
        {
            EntityEntry entry = this.Context.Entry(entity);

            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }
            else
            {
                this.Entites.Add(entity);
            }

           //this.SaveChanges();
        }

        public void Delete(T entity)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.Now;

            var entry = this.Context.Entry(entity);
            entry.State = EntityState.Modified;

            //this.SaveChanges();
        }

        public void Update(T entity)
        {
            EntityEntry entry = this.Context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                this.Entites.Attach(entity);
            }

            entry.State = EntityState.Modified;

            //this.SaveChanges();
        }
        public IQueryable<T> GetAll()
        {
            return this.Entites.Where(x => !x.IsDeleted);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> filter)
        {
            return this.GetAll().Where(filter);
        }

        public IQueryable<T> GetAllAndDeleted()
        {
            return this.Entites;
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter)
        {
            return this.Entites.FirstOrDefault(filter);
        }

        public T GetById(int id)
        {
            return this.Entites.Find(id);
        }

        //public IEnumerable<T> Find(Func<T, bool> filter)
        //{
        //    return this.Entites
        //               .Where(filter);
        //}

        public int Count(Func<T, bool> filter)
        {
            return this.Entites
                       .Where(filter)
                       .Count();
        }

        public bool IsSeeded()
        {
            return this.Entites.Any();
        }
    }
}
