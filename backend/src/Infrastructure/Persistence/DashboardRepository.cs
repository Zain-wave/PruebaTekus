using Microsoft.EntityFrameworkCore;
using PruebaTekus.Application.Dashboard;

namespace PruebaTekus.Infrastructure.Persistence;

public class DashboardRepository(AppDbContext context) : IDashboardRepository
{
    public async Task<DashboardSummaryDto> GetSummaryAsync(CancellationToken cancellationToken)
    {
        var totalProviders = await context.Providers.CountAsync(cancellationToken);
        var totalServices = await context.Services.CountAsync(cancellationToken);
        var averageHourlyRate = totalServices == 0
            ? 0m
            : await context.Services.AverageAsync(service => service.HourlyRate, cancellationToken);

        var providersByCountryRaw = await context.Providers
            .GroupBy(provider => provider.Country)
            .Select(group => new { Country = group.Key, Count = group.Count() })
            .OrderByDescending(item => item.Count)
            .ToListAsync(cancellationToken);

        var servicesByCountryRaw = await context.Services
            .GroupBy(service => service.Provider.Country)
            .Select(group => new { Country = group.Key, Count = group.Count() })
            .OrderByDescending(item => item.Count)
            .ToListAsync(cancellationToken);

        var providersByCountry = providersByCountryRaw
            .Select(item => new CountByCountryDto(item.Country, item.Count))
            .ToList();

        var servicesByCountry = servicesByCountryRaw
            .Select(item => new CountByCountryDto(item.Country, item.Count))
            .ToList();

        return new DashboardSummaryDto(
            totalProviders,
            totalServices,
            averageHourlyRate,
            providersByCountry,
            servicesByCountry);
    }
}
