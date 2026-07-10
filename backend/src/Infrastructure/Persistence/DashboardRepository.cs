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

        var providersByCountry = await context.Providers
            .GroupBy(provider => provider.Country)
            .Select(group => new CountByCountryDto(group.Key, group.Count()))
            .OrderByDescending(item => item.Count)
            .ToListAsync(cancellationToken);

        var servicesByCountry = await context.Services
            .GroupBy(service => service.Provider.Country)
            .Select(group => new CountByCountryDto(group.Key, group.Count()))
            .OrderByDescending(item => item.Count)
            .ToListAsync(cancellationToken);

        return new DashboardSummaryDto(
            totalProviders,
            totalServices,
            averageHourlyRate,
            providersByCountry,
            servicesByCountry);
    }
}
