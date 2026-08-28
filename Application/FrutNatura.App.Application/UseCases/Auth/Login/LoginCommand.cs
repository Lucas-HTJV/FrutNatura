using FrutNatura.Core.Contracts.Auth;
using MediatR;

namespace FrutNatura.App.Application.UseCases.Auth.Login;


public sealed record LoginCommand(string Email, string? Password = null) : IRequest<LoginResponse>;
