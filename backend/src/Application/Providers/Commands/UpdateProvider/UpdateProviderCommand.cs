using MediatR;

namespace PruebaTekus.Application.Providers.Commands.UpdateProvider;

public record UpdateProviderCommand(int Id, string Name, string Website, string Email, string Country) : IRequest;
