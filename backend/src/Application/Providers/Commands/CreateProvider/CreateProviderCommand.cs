using MediatR;

namespace PruebaTekus.Application.Providers.Commands.CreateProvider;

public record CreateProviderCommand(string Nit, string Name, string Website, string Email) : IRequest<int>;
