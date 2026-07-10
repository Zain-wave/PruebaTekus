using MediatR;
using Microsoft.AspNetCore.Mvc;
using PruebaTekus.Application.Products;
using PruebaTekus.Application.Products.Commands.CreateProduct;
using PruebaTekus.Application.Products.Queries.GetProductById;

namespace PruebaTekus.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="command">Name and price of the product to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The identifier of the created product.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<ActionResult<Guid>> Create(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var id = await sender.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>
    /// Gets a product by its identifier.
    /// </summary>
    /// <param name="id">Product identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var product = await sender.Send(new GetProductByIdQuery(id), cancellationToken);

        return product is null ? NotFound() : Ok(product);
    }
}
