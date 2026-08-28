namespace FrutNatura.Core.Abstractions.Common;


public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
    DateTime Today => UtcNow.Date;
}
