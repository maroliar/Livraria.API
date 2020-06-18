using Livraria.Domain.Contracts.Repositories;
using Livraria.Domain.Contracts.Services;
using Livraria.Domain.Entities;
using Livraria.Utils.ResourceFiles;
using System;

namespace Livraria.Domain.Services
{
    public class LivroDomainService : BaseDomainService<Livro, int>, ILivroDomainService
    {
        private readonly ILivroRepository repository;
        public LivroDomainService(ILivroRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public override void Insert(Livro obj)
        {
            if (HasISBN(obj.ISBN))
            {
                throw new Exception(LivroResource.ISBNJaCadastrado);
            }

            repository.Insert(obj);
        }

        protected bool HasISBN(int ISBN)
        {
            return repository.Count(x => x.ISBN.Equals(ISBN)) > 0;
        }
    }
}
