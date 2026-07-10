using MediatR;
using PruebaTekus.Application.Common;

namespace PruebaTekus.Application.Services.Queries.GetServicesList;

public class GetServicesListQueryHandler(IServiceRepository repository)
    : IRequestHandler<GetServicesListQuery, PagedResult<ServiceDto>>
{
    public async Task<PagedResult<ServiceDto>> Handle(GetServicesListQuery request, CancellationToken cancellationToken)
    {
        var pagedServices = await repository.GetPagedAsync(request, cancellationToken);

        return new PagedResult<ServiceDto>
        {
            Items = pagedServices.Items.Select(service => service.ToDto()).ToList(),
            Page = pagedServices.Page,
            PageSize = pagedServices.PageSize,
            TotalCount = pagedServices.TotalCount,
        };
    }
}
