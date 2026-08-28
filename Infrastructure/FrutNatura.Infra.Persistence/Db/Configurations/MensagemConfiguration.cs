using FrutNatura.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrutNatura.Infra.Persistence.Configurations;

public sealed class MensagemConfiguration : IEntityTypeConfiguration<Mensagem>
{
    public void Configure(EntityTypeBuilder<Mensagem> b)
    {
        b.ToTable("Mensagens");

        b.HasKey(x => x.Id);

        b.Property(x => x.ChamadoId)
            .IsRequired();

        b.Property(x => x.AutorId)
            .IsRequired(false);

        
        b.Property(x => x.Conteudo)
            .HasMaxLength(4000)
            .IsRequired();

        b.Property(x => x.CriadoEmUtc)
            .HasPrecision(0);

        b.HasIndex(x => x.ChamadoId);
    }
}
