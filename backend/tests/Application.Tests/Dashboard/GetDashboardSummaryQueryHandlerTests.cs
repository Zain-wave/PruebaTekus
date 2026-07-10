using PruebaTekus.Application.Dashboard;
using PruebaTekus.Application.Dashboard.Queries.GetDashboardSummary;
using PruebaTekus.Application.Tests.TestDoubles;

namespace PruebaTekus.Application.Tests.Dashboard;

public class GetDashboardSummaryQueryHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsSummaryFromRepository()
    {
        var repository = new FakeDashboardRepository
        {
            Summary = new DashboardSummaryDto(
                TotalProviders: 5,
                TotalServices: 8,
                AverageHourlyRate: 92.5m,
                ProvidersByCountry: [new CountByCountryDto("Colombia", 2), new CountByCountryDto("Mexico", 1)],
                ServicesByCountry: [new CountByCountryDto("Colombia", 4)]),
        };
        var handler = new GetDashboardSummaryQueryHandler(repository);

        var result = await handler.Handle(new GetDashboardSummaryQuery(), CancellationToken.None);

        Assert.Equal(5, result.TotalProviders);
        Assert.Equal(8, result.TotalServices);
        Assert.Equal(92.5m, result.AverageHourlyRate);
        Assert.Equal(2, result.ProvidersByCountry.Count);
        Assert.Single(result.ServicesByCountry);
    }
}
