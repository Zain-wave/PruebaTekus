using MediatR;

namespace PruebaTekus.Application.Services.Queries.GetServiceById;

public record GetServiceByIdQuery(int Id) : IRequest<ServiceDto?>;
