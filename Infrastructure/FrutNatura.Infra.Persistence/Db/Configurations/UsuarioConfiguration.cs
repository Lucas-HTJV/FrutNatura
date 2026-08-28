using FrutNatura.Core.Domain.Entities;
using FrutNatura.Core.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace FrutNatura.Infra.Persistence.Db.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> b)
    {
        b.ToTable("Usuarios");
        b.HasKey(x => x.Id);

        b.Property(x => x.Nome)
            .HasMaxLength(120)
            .IsRequired();

      
        b.Property(x => x.Email)
            .HasConversion(new ValueConverter<Email, string>(
                v => v.Valor,
                v => Email.From(v)))
            .HasMaxLength(256)
            .IsRequired();

        b.Property(x => x.PasswordHash)
            .HasMaxLength(500)
            .IsRequired();

        b.Property(x => x.Ativo)
            .IsRequired();

        b.Property(x =>x.RolesSerialized)
         .HasColumnName("Roles")
         .HasMaxLength(512);

        b.Ignore("_roles");
        b.Ignore(x => x.Roles);

        b.HasIndex(x => x.Email)
            .IsUnique();

        b.HasMany(u => u.RefreshTokens)
            .WithOne(rt => rt.Usuario)
            .HasForeignKey(rt => rt.UsuarioId)       
            .OnDelete(DeleteBehavior.Cascade);
    }
}
