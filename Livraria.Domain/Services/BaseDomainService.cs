using Livraria.Domain.Contracts.Repositories;
using Livraria.Domain.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Livraria.Domain.Services
{
    public abstract class BaseDomainService<TEntity, TKey> : IBaseDomainService<TEntity, TKey> where TEntity : class
    {
        private readonly IBaseRepository<TEntity, TKey> repository;

        protected BaseDomainService(IBaseRepository<TEntity, TKey> repository)
        {
            this.repository = repository;
        }

        public virtual void Insert(TEntity obj)
        {
            repository.Insert(obj);
        }
        public virtual void Update(TEntity obj)
        {
            repository.Update(obj);
        }
        public virtual void Delete(TEntity obj)
        {
            repository.Delete(obj);
        }

        public virtual TEntity Find(Func<TEntity, bool> predicate)
        {
            return repository.Find(predicate);
        }
        public virtual TEntity FindById(TKey id)
        {
            return repository.FindById(id);
        }

        public virtual ICollection<TEntity> FindAll()
        {
            return repository.FindAll();
        }
        public ICollection<TEntity> FindAll(Func<TEntity, bool> predicate)
        {
            return repository.FindAll(predicate);
        }

        public virtual int Count(Func<TEntity, bool> predicate)
        {
            return repository.Count(predicate);
        }
        
        public virtual void Dispose()
        {
            repository.Dispose();
        }
    }
}
