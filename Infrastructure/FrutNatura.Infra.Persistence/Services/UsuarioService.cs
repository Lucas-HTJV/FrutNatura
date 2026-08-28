using FrutNatura.Core.Abstractions.Repositories;
using FrutNatura.Core.Abstractions.Services;
using FrutNatura.Core.Domain.Entities;
using FrutNatura.Infra.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrutNatura.Infra.Services
{
    public class UsuarioService
    {
        private readonly IUsuariosRepository _usuarioRepository;

        public UsuarioService(IUsuariosRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task ConcederRoleAoUsuario(Guid usuarioId, string role)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario != null)
            {
                usuario.ConcederRole(role);
                await _usuarioRepository.Save(usuario);
            }
        }

        public async Task RevogarRoleDeUsuario(Guid usuarioId, string role)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario != null)
            {
                usuario.RevogarRole(role);
                await _usuarioRepository.Save(usuario);
            }
        }


    }

}
