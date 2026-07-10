using MediatR;
using PruebaTekus.Application.Common.Exceptions;
using PruebaTekus.Domain.Entities;

namespace PruebaTekus.Application.Providers.Commands.CreateProvider;

public class CreateProviderCommandHandler(IProviderRepository repository) : IRequestHandler<CreateProviderCommand, int>
{
    public async Task<int> Handle(CreateProviderCommand request, CancellationToken cancellationToken)
    {
        if (await repository.ExistsByNitAsync(request.Nit, excludeId: null, cancellationToken))
        {
            throw new ConflictException($"A provider with Nit '{request.Nit}' already exists.");
        }

        var provider = new Provider(request.Nit, request.Name, request.Website, request.Email, request.Country);

        await repository.AddAsync(provider, cancellationToken);

        return provider.Id;
    }
}
