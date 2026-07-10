using MediatR;
using PruebaTekus.Application.Common;

namespace PruebaTekus.Application.Providers.Queries.GetProvidersList;

public class GetProvidersListQueryHandler(IProviderRepository repository)
    : IRequestHandler<GetProvidersListQuery, PagedResult<ProviderDto>>
{
    public async Task<PagedResult<ProviderDto>> Handle(GetProvidersListQuery request, CancellationToken cancellationToken)
    {
        var pagedProviders = await repository.GetPagedAsync(request, cancellationToken);

        return new PagedResult<ProviderDto>
        {
            Items = pagedProviders.Items.Select(provider => provider.ToDto()).ToList(),
            Page = pagedProviders.Page,
            PageSize = pagedProviders.PageSize,
            TotalCount = pagedProviders.TotalCount,
        };
    }
}
