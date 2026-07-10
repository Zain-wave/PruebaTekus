using MediatR;
using PruebaTekus.Application.Common.Exceptions;
using PruebaTekus.Domain.Entities;

namespace PruebaTekus.Application.Services.Commands.UpdateService;

public class UpdateServiceCommandHandler(IServiceRepository repository) : IRequestHandler<UpdateServiceCommand>
{
    public async Task Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Service), request.Id);

        service.Update(request.Name, request.HourlyRate);

        await repository.UpdateAsync(service, cancellationToken);
    }
}
