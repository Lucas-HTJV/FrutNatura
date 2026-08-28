
using FrutNatura.Core.Domain.Entities;
using FrutNatura.Infra.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;



namespace FrutNatura.Infra.Persistence.Db;

public class FrutNaturaDbContext : DbContext
{
    public DbSet<Chamado> Chamados => Set<Chamado>();
    public DbSet<Mensagem> Mensagens => Set<Mensagem>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<RefreshToken> RefreshToken => Set<RefreshToken>();

    public FrutNaturaDbContext(DbContextOptions<FrutNaturaDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FrutNatura.Infra.Persistence.Db.Configurations.RefreshTokenConfiguration).Assembly);
       
    }
}
