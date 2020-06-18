using Livraria.Domain.Contracts.Repositories;
using Livraria.Domain.Entities;
using Livraria.Infra.Data.Context;

namespace Livraria.Infra.Data.Repositories
{
    public class UsuarioRepository : BaseRepository<Usuario, int>, IUsuarioRepository
    {
        private readonly DataContext context;

        public UsuarioRepository(DataContext context) : base(context)
        {
            this.context = context;
        }
    }
}
