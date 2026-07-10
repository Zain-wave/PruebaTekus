using PruebaTekus.Application.Dashboard;

namespace PruebaTekus.Application.Tests.TestDoubles;

public class FakeDashboardRepository : IDashboardRepository
{
    public DashboardSummaryDto Summary { get; set; } = new(
        TotalProviders: 0,
        TotalServices: 0,
        AverageHourlyRate: 0m,
        ProvidersByCountry: Array.Empty<CountByCountryDto>(),
        ServicesByCountry: Array.Empty<CountByCountryDto>());

    public Task<DashboardSummaryDto> GetSummaryAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(Summary);
    }
}
