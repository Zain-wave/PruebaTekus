using MediatR;
using Microsoft.AspNetCore.Mvc;
using PruebaTekus.Application.Common;
using PruebaTekus.Application.Providers;
using PruebaTekus.Application.Providers.Commands.CreateProvider;
using PruebaTekus.Application.Providers.Commands.UpdateProvider;
using PruebaTekus.Application.Providers.Queries.GetProviderById;
using PruebaTekus.Application.Providers.Queries.GetProvidersList;

namespace PruebaTekus.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProvidersController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Gets a paged, searchable and sortable list of providers.
    /// </summary>
    /// <param name="query">Pagination, search and sort parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<ProviderDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<ProviderDto>>> GetList(
        [FromQuery] GetProvidersListQuery query,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(query, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Gets a provider by its identifier.
    /// </summary>
    /// <param name="id">Provider identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProviderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProviderDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var provider = await sender.Send(new GetProviderByIdQuery(id), cancellationToken);

        return provider is null ? NotFound() : Ok(provider);
    }

    /// <summary>
    /// Creates a new provider.
    /// </summary>
    /// <param name="command">Nit, name, website and email of the provider to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The identifier of the created provider.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<int>> Create(CreateProviderCommand command, CancellationToken cancellationToken)
    {
        var id = await sender.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>
    /// Updates an existing provider.
    /// </summary>
    /// <param name="id">Provider identifier.</param>
    /// <param name="command">Name, website and email to update.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, UpdateProviderCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest("Route id and body id must match.");
        }

        await sender.Send(command, cancellationToken);

        return NoContent();
    }
}
