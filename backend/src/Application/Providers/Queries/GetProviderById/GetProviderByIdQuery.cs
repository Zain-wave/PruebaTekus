using MediatR;

namespace PruebaTekus.Application.Providers.Queries.GetProviderById;

public record GetProviderByIdQuery(int Id) : IRequest<ProviderDto?>;
