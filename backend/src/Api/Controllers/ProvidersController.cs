using MediatR;
using Microsoft.AspNetCore.Mvc;
using PruebaTekus.Application.Providers.Commands.CreateProvider;
using PruebaTekus.Application.Providers.Commands.UpdateProvider;

namespace PruebaTekus.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProvidersController(ISender sender) : ControllerBase
{
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

        return StatusCode(StatusCodes.Status201Created, id);
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
