using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using FrutNatura.Core.Abstractions.Repositories;
using FrutNatura.Core.Domain.Entities;
using FrutNatura.Infra.Persistence.Db;
using Microsoft.EntityFrameworkCore;

namespace FrutNatura.Infra.Persistence
{
    public sealed class ChamadosRepository : IChamadosRepository
    {
        private readonly FrutNaturaDbContext _ctx;

        public ChamadosRepository(FrutNaturaDbContext ctx)
        {
            _ctx = ctx;
        }

        // =========================================================
        // Métodos básicos de repositório
        // =========================================================

        public async Task<Chamado?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _ctx
                .Set<Chamado>()
                .FirstOrDefaultAsync(c => c.Id == id, ct);
        }

        public async Task<IReadOnlyList<Chamado>> ListAsync(
            ISpecification<Chamado> spec,
            CancellationToken ct = default)
        {
            var query = SpecificationEvaluator.Default
                .GetQuery(_ctx.Set<Chamado>().AsQueryable(), spec);

            return await query.ToListAsync(ct);
        }

        public async Task<int> CountAsync(
            ISpecification<Chamado> spec,
            CancellationToken ct = default)
        {
            var query = SpecificationEvaluator.Default
                .GetQuery(_ctx.Set<Chamado>().AsQueryable(), spec);

            return await query.CountAsync(ct);
        }

        public async Task AddAsync(Chamado entity, CancellationToken ct = default)
        {
            await _ctx.Set<Chamado>().AddAsync(entity, ct);
            
        }

        public async Task UpdateAsync(Chamado entity, CancellationToken ct = default)
        {
            _ctx.Set<Chamado>().Update(entity);
           
        }

        // =========================================================
        // Regras específicas
        // =========================================================

        /// <summary>
        /// Salva um novo chamado. A entidade já deve vir montada
        /// com ClienteId, Título, Descrição, etc.
        /// </summary>
        public async Task AbrirAsync(Chamado chamado, CancellationToken ct = default)
        {
            // aqui usamos o AddAsync já existente
            await AddAsync(chamado, ct);
        }

        /// <summary>
        /// Atribui um responsável (usuário técnico) ao chamado.
        /// </summary>
        public async Task AtribuirResponsavelAsync(
            Guid chamadoId,
            Guid responsavelId,
            CancellationToken ct = default)
        {
            var chamado = await _ctx.Chamados
                .FirstOrDefaultAsync(c => c.Id == chamadoId, ct);

            if (chamado is null)
                return; // ou lançar exceção, se preferir

            // método de domínio (vamos criar já já em Chamado.cs)
            chamado.AtribuirResponsavel(responsavelId);

            await _ctx.SaveChangesAsync(ct);
        }

        /// <summary>
        /// Lista os chamados de um cliente específico com paginação simples.
        /// </summary>
        public async Task<List<Chamado>> ListarDoClienteAsync(
            Guid clienteId,
            int page,
            int pageSize,
            CancellationToken ct = default)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            return await _ctx.Chamados
                .Where(c => c.ClienteId == clienteId)
                .OrderByDescending(c => c.CriadoEmUtc)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
        }
    }
}
