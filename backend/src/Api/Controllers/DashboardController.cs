using MediatR;
using Microsoft.AspNetCore.Mvc;
using PruebaTekus.Application.Dashboard;
using PruebaTekus.Application.Dashboard.Queries.GetDashboardSummary;

namespace PruebaTekus.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Gets a summary of relevant indicators: totals, average hourly rate, and providers/services grouped by country.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet("summary")]
    [ProducesResponseType(typeof(DashboardSummaryDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<DashboardSummaryDto>> GetSummary(CancellationToken cancellationToken)
    {
        var summary = await sender.Send(new GetDashboardSummaryQuery(), cancellationToken);

        return Ok(summary);
    }
}
