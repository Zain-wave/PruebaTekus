using MediatR;

namespace PruebaTekus.Application.Providers.Queries.GetProviderById;

public class GetProviderByIdQueryHandler(IProviderRepository repository) : IRequestHandler<GetProviderByIdQuery, ProviderDto?>
{
    public async Task<ProviderDto?> Handle(GetProviderByIdQuery request, CancellationToken cancellationToken)
    {
        var provider = await repository.GetByIdAsync(request.Id, cancellationToken);

        return provider?.ToDto();
    }
}
