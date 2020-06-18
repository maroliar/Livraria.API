using System;
using System.Collections.Generic;

namespace Livraria.Domain.Contracts.Services
{
    public interface IBaseDomainService<TEntity, TKey> : IDisposable where TEntity : class
    {
        void Insert(TEntity obj);
        void Update(TEntity obj);
        void Delete(TEntity obj);
        
        TEntity Find(Func<TEntity, bool> predicate);
        TEntity FindById(TKey id);
        
        ICollection<TEntity> FindAll();
        ICollection<TEntity> FindAll(Func<TEntity, bool> predicate);
        
        int Count(Func<TEntity, bool> predicate);
    }
}
