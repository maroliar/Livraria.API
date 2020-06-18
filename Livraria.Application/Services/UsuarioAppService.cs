using AutoMapper;
using Livraria.Application.Contracts;
using Livraria.Application.Models.Usuario;
using Livraria.Domain.Contracts.Services;
using Livraria.Domain.Entities;

namespace Livraria.Application.Services
{
    public class UsuarioAppService : IUsuarioAppService
    {
        private readonly IUsuarioDomainService domain;
        private readonly IMapper mapper;

        public UsuarioAppService(IUsuarioDomainService domain, IMapper mapper)
        {
            this.domain = domain;
            this.mapper = mapper;
        }

        public Usuario FindByLoginAndSenha(UsuarioLoginModel model)
        {
            return domain.FindByLoginAndPassword(model.Login, model.Senha);
        }

        public void Insert(UsuarioCadastroModel model)
        {
            var usuario = mapper.Map<Usuario>(model);
            domain.Insert(usuario);
        }

        public void Dispose()
        {
            domain.Dispose();
        }
    }
}
