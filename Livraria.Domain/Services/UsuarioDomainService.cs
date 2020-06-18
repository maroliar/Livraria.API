using Livraria.Domain.Contracts.Repositories;
using Livraria.Domain.Contracts.Services;
using Livraria.Domain.Entities;
using Livraria.Utils;
using Livraria.Utils.ResourceFiles;
using System;

namespace Livraria.Domain.Services
{
    public class UsuarioDomainService : BaseDomainService<Usuario, int>, IUsuarioDomainService
    {
        private readonly ICryptorEngine cryptorEngine;
        private readonly IUsuarioRepository repository;

        public UsuarioDomainService(ICryptorEngine cryptorEngine, IUsuarioRepository repository) : base(repository)
        {
            this.cryptorEngine = cryptorEngine;
            this.repository = repository;
        }

        public override void Insert(Usuario obj)
        {
            obj.Senha = cryptorEngine.Encrypt(obj.Senha, true);

            if (HasLogin(obj.Login))
            {
                throw new Exception(UsuarioResource.LoginJaEmUso);
            }

            repository.Insert(obj);
        }

        protected bool HasLogin(string login)
        {
            return repository.Count(x => x.Login.Equals(login)) > 0;
        }

        public Usuario FindByLoginAndPassword(string login, string senha)
        {
            senha = cryptorEngine.Encrypt(senha, true);
            return repository.Find(x => x.Login == login && x.Senha == senha);
        }
    }
}
