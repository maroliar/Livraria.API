using Livraria.Domain.Entities;

namespace Livraria.Domain.Contracts.Services
{
    public interface IUsuarioDomainService : IBaseDomainService<Usuario, int>
    {
        Usuario FindByLoginAndPassword(string login, string senha);
    }
}
