using MediatR;

namespace PruebaTekus.Application.Services.Queries.GetServiceById;

public class GetServiceByIdQueryHandler(IServiceRepository repository) : IRequestHandler<GetServiceByIdQuery, ServiceDto?>
{
    public async Task<ServiceDto?> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
    {
        var service = await repository.GetByIdAsync(request.Id, cancellationToken);

        return service?.ToDto();
    }
}
