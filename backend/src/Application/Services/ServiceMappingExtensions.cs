using PruebaTekus.Domain.Entities;

namespace PruebaTekus.Application.Services;

public static class ServiceMappingExtensions
{
    public static ServiceDto ToDto(this Service service)
    {
        return new ServiceDto(
            service.Id,
            service.Name,
            service.HourlyRate,
            service.ProviderId,
            service.Provider.Name);
    }
}
