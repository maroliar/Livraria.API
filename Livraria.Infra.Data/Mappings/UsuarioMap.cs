using Livraria.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Livraria.Infra.Data.Mappings
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(u => new { u.IdUsuario });

            builder.Property(u => u.IdUsuario)
                .HasColumnName("IdUsuario");

            builder.Property(u => u.Nome)
               .HasColumnName("Nome")
               .HasMaxLength(100)
               .IsRequired();

            builder.Property(u => u.Login)
               .HasColumnName("Login")
               .HasMaxLength(50)
               .IsRequired();

            builder.Property(u => u.Senha)
               .HasColumnName("Senha")
               .HasMaxLength(50)
               .IsRequired();
        }
    }
}
