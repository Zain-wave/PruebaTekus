using MediatR;
using PruebaTekus.Application.Common;

namespace PruebaTekus.Application.Services.Queries.GetServicesList;

public class GetServicesListQuery : PagedRequest, IRequest<PagedResult<ServiceDto>>
{
}
