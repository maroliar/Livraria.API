using Livraria.Application.Models.Usuario;
using Livraria.Domain.Entities;
using System;

namespace Livraria.Application.Contracts
{
    public interface IUsuarioAppService : IDisposable
    {
        void Insert(UsuarioCadastroModel model);
        Usuario FindByLoginAndSenha(UsuarioLoginModel model);
    }
}
