using System.Linq.Expressions;

namespace FrutNatura.Core.Abstractions.Common;

/// <summary>
/// Padrão Specification para compor filtros de repositórios.
/// </summary>
public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }
}
