using Livraria.Domain.Entities;
using Livraria.Infra.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Livraria.Infra.Data.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> builder) : base(builder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new LivroMap());

            modelBuilder.Entity<Livro>().HasAlternateKey(u => u.ISBN);
        }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Livro> Livro { get; set; }
    }
}
