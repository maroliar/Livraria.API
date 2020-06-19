using Livraria.Domain.Contracts.Identity;
using Livraria.Domain.Contracts.Repositories;
using Livraria.Domain.Contracts.Services;
using Livraria.Domain.Entities;
using Livraria.Utils.ResourceFiles;
using System;
using System.Linq;

namespace Livraria.Domain.Services
{
    public class LivroDomainService : BaseDomainService<Livro, int>, ILivroDomainService
    {
        private readonly ILivroRepository repository;
        private readonly IUser loggedUser;

        public LivroDomainService(ILivroRepository repository, IUser loggedUser) : base(repository)
        {
            this.repository = repository;
            this.loggedUser = loggedUser;
        }

        public override void Insert(Livro obj)
        {
            obj.IdUsuario = ObterUsuarioLogado();

            if (HasISBN(obj.ISBN))
            {
                throw new Exception(LivroResource.ISBNJaCadastrado);
            }

            repository.Insert(obj);
        }

        public override void Update(Livro obj)
        {
            obj.IdUsuario = ObterUsuarioLogado();

            repository.Update(obj);
        }

        protected bool HasISBN(int ISBN)
        {
            return repository.Count(x => x.ISBN.Equals(ISBN)) > 0;
        }

        protected int ObterUsuarioLogado()
        {
            try
            {
                var idUsuario = loggedUser.GetUserAuthenticatedId().First();

                return Convert.ToInt16(idUsuario);
            }
            catch (Exception)
            {
                throw new Exception(UsuarioResource.ErroObterUsuarioLogado);
            }
        }
    }
}
