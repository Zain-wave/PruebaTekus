using MediatR;
using Microsoft.AspNetCore.Mvc;
using PruebaTekus.Application.Common;
using PruebaTekus.Application.Services;
using PruebaTekus.Application.Services.Commands.CreateService;
using PruebaTekus.Application.Services.Commands.UpdateService;
using PruebaTekus.Application.Services.Queries.GetServiceById;
using PruebaTekus.Application.Services.Queries.GetServicesList;

namespace PruebaTekus.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicesController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Gets a paged, searchable and sortable list of services.
    /// </summary>
    /// <param name="query">Pagination, search and sort parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<ServiceDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<ServiceDto>>> GetList(
        [FromQuery] GetServicesListQuery query,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(query, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Gets a service by its identifier.
    /// </summary>
    /// <param name="id">Service identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ServiceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ServiceDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var service = await sender.Send(new GetServiceByIdQuery(id), cancellationToken);

        return service is null ? NotFound() : Ok(service);
    }

    /// <summary>
    /// Creates a new service for a provider. Sends a notification email once created.
    /// </summary>
    /// <param name="command">Name, hourly rate and provider identifier of the service to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The identifier of the created service.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<int>> Create(CreateServiceCommand command, CancellationToken cancellationToken)
    {
        var id = await sender.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>
    /// Updates an existing service.
    /// </summary>
    /// <param name="id">Service identifier.</param>
    /// <param name="command">Name and hourly rate to update.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, UpdateServiceCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest("Route id and body id must match.");
        }

        await sender.Send(command, cancellationToken);

        return NoContent();
    }
}
