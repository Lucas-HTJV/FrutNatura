using FrutNatura.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrutNatura.Infra.Persistence.Db.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> b)
        {
            b.ToTable("RefreshTokens");
            b.HasKey(x => x.Id);

            b.Property(x => x.Token)
                .HasMaxLength(500)
                .IsRequired();

            // Datas em UTC

            b.Property(x => x.CreatedUtc)
                .HasColumnName("CriadoEmUtc")
                .IsRequired();

            b.Property(x => x.ExpiresUtc)
                .HasColumnName("ExpiraEmUtc")
                .IsRequired();

            b.Property(x => x.RevokedUtc)
                .HasColumnName("RevogadoEmUtc")
                .IsRequired(false); 

            // UserId
            
            b.Property(x => x.UsuarioId)
                .HasColumnName("UsuarioId")
                .IsRequired();                     

            // 🔹 Relacionamento com Usuário (1:N)
            b.HasOne(x => x.Usuario)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(x => x.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_RefreshTokens_Usuarios");

            // 🔹 Índice para buscas rápidas por Token e UserId
            b.HasIndex(x => x.Token).IsUnique();
            b.HasIndex(x => x.UsuarioId);
        }
    }
}