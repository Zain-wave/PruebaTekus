using MediatR;
using PruebaTekus.Application.Common;

namespace PruebaTekus.Application.Providers.Queries.GetProvidersList;

public class GetProvidersListQuery : PagedRequest, IRequest<PagedResult<ProviderDto>>
{
}
