namespace PruebaTekus.Application.Dashboard;

public interface IDashboardRepository
{
    Task<DashboardSummaryDto> GetSummaryAsync(CancellationToken cancellationToken);
}
