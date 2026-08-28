using FrutNatura.Core.Domain.Entities;
using FrutNatura.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrutNatura.Infra.Persistence.Db.Configurations;

public class ChamadoConfiguration : IEntityTypeConfiguration<Chamado>
{
    public void Configure(EntityTypeBuilder<Chamado> b)
    {
        b.ToTable("Chamados");
        b.HasKey(x => x.Id);

        b.Property(x => x.ClienteId)      
         .IsRequired();

        b.Property(x => x.Status)
         .HasConversion<string>()
         .HasMaxLength(32)
         .IsRequired();

        b.Property(x => x.Titulo).HasMaxLength(120).IsRequired();
        b.Property(x => x.Descricao).HasMaxLength(4000).IsRequired();
    }
}
