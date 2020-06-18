using Livraria.Domain.Contracts.Repositories;
using Livraria.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Livraria.Infra.Data.Repositories
{
    public abstract class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey> where TEntity : class
    {
        private readonly DataContext context;

        public BaseRepository(DataContext context)
        {
            this.context = context;
        }

        public void Insert(TEntity obj)
        {
            context.Entry(obj).State = EntityState.Added;
            context.SaveChanges();
        }
        public void Update(TEntity obj)
        {
            context.Entry(obj).State = EntityState.Modified;
            context.SaveChanges();
        }
        public void Delete(TEntity obj)
        {
            context.Entry(obj).State = EntityState.Deleted;
            context.SaveChanges();
        }

        public TEntity Find(Func<TEntity, bool> predicate)
        {
            return context.Set<TEntity>().FirstOrDefault(predicate);
        }
        public TEntity FindById(TKey id)
        {
            return context.Set<TEntity>().Find(id);
        }

        public ICollection<TEntity> FindAll()
        {
            return context.Set<TEntity>().ToList();
        }
        public ICollection<TEntity> FindAll(Func<TEntity, bool> predicate)
        {
            return context.Set<TEntity>().Where(predicate).ToList();
        }

        public int Count(Func<TEntity, bool> predicate)
        {
            return context.Set<TEntity>().Count(predicate);
        }

        public void Dispose()
        {
            context.Dispose();
        }      
    }
}