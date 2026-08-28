using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FrutNatura.Core.Domain.Entities;

namespace FrutNatura.Core.Abstractions.Services
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> ObterUsuariosPorRoleAsync(string role);  // Consultar usuários por role
        Task<Usuario?> ObterUsuarioPorIdAsync(Guid usuarioId);       // Consultar usuário por ID
        Task AtribuirResponsavelAoChamadoAsync(Guid chamadoId, Guid responsavelId); // Atribuir responsável
    }
}
