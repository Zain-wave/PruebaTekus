using MediatR;

namespace PruebaTekus.Application.Services.Commands.UpdateService;

public record UpdateServiceCommand(int Id, string Name, decimal HourlyRate) : IRequest;
