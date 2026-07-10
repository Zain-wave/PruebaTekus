using PruebaTekus.Domain.Entities;

namespace PruebaTekus.Application.Providers;

public static class ProviderMappingExtensions
{
    public static ProviderDto ToDto(this Provider provider)
    {
        return new ProviderDto(
            provider.Id,
            provider.Nit,
            provider.Name,
            provider.Website,
            provider.Email,
            provider.Services.Count);
    }
}
