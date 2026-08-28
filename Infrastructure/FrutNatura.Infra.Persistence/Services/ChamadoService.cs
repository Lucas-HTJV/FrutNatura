using FrutNatura.App.Application.UseCases.ListarPorCliente;
using FrutNatura.Core.Abstractions.Common.PageResults;
using FrutNatura.Core.Abstractions.Repositories;
using FrutNatura.Core.Abstractions.Services;
using FrutNatura.Core.Contracts.Chamados;
using FrutNatura.Core.Domain.Entities;
using FrutNatura.Infra.Persistence;

namespace FrutNatura.Infra.Persistence.Services
{
    public sealed class ChamadoService : IChamadosService
    {
        private readonly IChamadosRepository _chamadoRepository;

        public ChamadoService(IChamadosRepository chamadoRepository)
        {
            _chamadoRepository = chamadoRepository;
        }

       
        public async Task<Guid> AbrirAsync(Guid clienteId, string titulo, string descricao, CancellationToken ct = default)
        {
            var chamado = Chamado.Abrir(clienteId, titulo, descricao); 
            await _chamadoRepository.AddAsync(chamado, ct);
           
            return chamado.Id;
        }


        public async Task<Chamado> ObterChamadoAsync(Guid id, CancellationToken ct)
        {
            return await _chamadoRepository.GetByIdAsync(id, ct);
        }

        public async Task AtualizarChamadoAsync(Chamado chamado, CancellationToken ct)
        {
            await _chamadoRepository.UpdateAsync(chamado, ct);
        }

       

        public async Task AtribuirAsync(Guid chamadoId, Guid responsavelId, CancellationToken ct)
        {
            await _chamadoRepository.AtribuirResponsavelAsync(chamadoId, responsavelId, ct);
        }




        public async Task AtribuirAsync(Guid chamadoId, Guid? responsavelId, CancellationToken ct = default)
        {
            if (!responsavelId.HasValue)
            {
                // desatribuir
                var chamado = await _chamadoRepository.GetByIdAsync(chamadoId, ct);
                if (chamado is null) return;
                chamado.Desatribuir();
                await _chamadoRepository.UpdateAsync(chamado, ct);
                return;
            }

            await _chamadoRepository.AtribuirResponsavelAsync(chamadoId, responsavelId.Value, ct);
        }

        public async Task<PagedResult<ChamadoListDto>> ListarDoClienteAsync(Guid clienteId, int page, int pageSize, CancellationToken ct = default)
        {
            var spec = new ChamadosPorClientePagedSpec(clienteId, null, null, page, pageSize); 

            var countSpec = new ChamadosPorClienteCountSpec(clienteId, null, null);            

            var entities = await _chamadoRepository.ListAsync(spec, ct);
            var total = await _chamadoRepository.CountAsync(countSpec, ct);

            var items = entities.Select(c => new ChamadoListDto
            {
                Id = c.Id,
                ClienteId = c.ClienteId,
                Titulo = c.Titulo,
                Status = c.Status.ToString(),
                CriadoEmUtc = c.CriadoEmUtc,
                Prioridade = c.Prioridade.ToString()
            }).ToList();

            return new PagedResult<ChamadoListDto>(items, total, page, pageSize);
        }
    }
}
