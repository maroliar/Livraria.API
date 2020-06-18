using Livraria.Domain.Contracts.Repositories;
using Livraria.Domain.Entities;
using Livraria.Infra.Data.Context;

namespace Livraria.Infra.Data.Repositories
{
    public class LivroRepository : BaseRepository<Livro, int>, ILivroRepository
    {
        private readonly DataContext context;

        public LivroRepository(DataContext context) : base(context)
        {
            this.context = context;
        }
    }
}
