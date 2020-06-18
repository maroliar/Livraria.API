using Livraria.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Livraria.Infra.Data.Mappings
{
    public class LivroMap : IEntityTypeConfiguration<Livro>
    {
        public void Configure(EntityTypeBuilder<Livro> builder)
        {
            builder.HasKey(l => l.IdLivro);

            builder.Property(l => l.ISBN)
                    .HasMaxLength(50)
                    .IsRequired();

            builder.Property(l => l.Autor)
                    .HasMaxLength(100)
                    .IsRequired();

            builder.Property(l => l.Nome)
                    .HasMaxLength(150)
                    .IsRequired();

            builder.Property(l => l.Preco)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

            builder.Property(l => l.DataPublicacao)
                    .HasColumnType("datetime")
                    .IsRequired();

            builder.Property(l => l.ImagemCapa)
                    .HasColumnType("varbinary(max)");


            // One to Many relationship..

            builder.HasOne(u => u.Usuario)
                    .WithMany(l => l.Livros)
                    .HasForeignKey(u => u.IdUsuario)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
