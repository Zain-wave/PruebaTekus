using MediatR;
using PruebaTekus.Application.Common.Exceptions;
using PruebaTekus.Domain.Entities;

namespace PruebaTekus.Application.Providers.Commands.UpdateProvider;

public class UpdateProviderCommandHandler(IProviderRepository repository) : IRequestHandler<UpdateProviderCommand>
{
    public async Task Handle(UpdateProviderCommand request, CancellationToken cancellationToken)
    {
        var provider = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(Provider), request.Id);

        provider.Update(request.Name, request.Website, request.Email, request.Country);

        await repository.UpdateAsync(provider, cancellationToken);
    }
}
