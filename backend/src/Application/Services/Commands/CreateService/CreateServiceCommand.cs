using MediatR;

namespace PruebaTekus.Application.Services.Commands.CreateService;

public record CreateServiceCommand(string Name, decimal HourlyRate, int ProviderId) : IRequest<int>;
