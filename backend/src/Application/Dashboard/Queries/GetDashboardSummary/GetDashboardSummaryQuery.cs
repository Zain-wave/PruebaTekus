using MediatR;

namespace PruebaTekus.Application.Dashboard.Queries.GetDashboardSummary;

public record GetDashboardSummaryQuery : IRequest<DashboardSummaryDto>;
