using FrutNatura.Core.Domain.ValueObjects;

namespace FrutNatura.Core.Domain.DomainServices;

public interface IProtocoloService
{
    Protocolo Gerar();
    bool Existe(string protocolo); // caso precise checar unicidade no repositório
}
