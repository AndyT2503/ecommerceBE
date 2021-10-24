using Ecommerce.Application.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var dto = await _mediator.Send(command, cancellationToken);
            return Ok(dto);
        }

        [HttpGet]
        public async Task<IActionResult> GetProduct([FromQuery] GetProductQuery query, CancellationToken cancellationToken)
        {
            var dto = await _mediator.Send(query, cancellationToken);
            return Ok(dto);
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetProductDetail(string slug, CancellationToken cancellationToken)
        {
            var dto = await _mediator.Send(new GetProductDetailQuery(slug), cancellationToken);
            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id, CancellationToken cancellationToken)
        {
            var dto = await _mediator.Send(new DeleteProductCommand(id), cancellationToken);
            return Ok(dto);
        }
    }
}
