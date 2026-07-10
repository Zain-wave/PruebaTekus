using MediatR;

namespace PruebaTekus.Application.Dashboard.Queries.GetDashboardSummary;

public class GetDashboardSummaryQueryHandler(IDashboardRepository repository)
    : IRequestHandler<GetDashboardSummaryQuery, DashboardSummaryDto>
{
    public Task<DashboardSummaryDto> Handle(GetDashboardSummaryQuery request, CancellationToken cancellationToken)
    {
        return repository.GetSummaryAsync(cancellationToken);
    }
}
