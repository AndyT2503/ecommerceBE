using Ecommerce.Application.ProductTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.API.Controllers
{
    [Route("api/product-type")]
    [ApiController]
    public class ProductTypeController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateProductType(CreateProductTypeCommand command, CancellationToken cancellationToken)
        {
            var res = await _mediator.Send(command, cancellationToken);
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductType(Guid id, CancellationToken cancellationToken)
        {
            var res = await _mediator.Send(new DeleteProductTypeCommand(id), cancellationToken);
            return Ok(res);
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateProductType(UpdateProductTypeCommand command, CancellationToken cancellationToken)
        {
            var res = await _mediator.Send(command, cancellationToken);
            return Ok(res);
        }

        [HttpGet()]
        public async Task<IActionResult> GetProductTypes([FromQuery] GetProductTypeQuery query, CancellationToken cancellationToken)
        {
            var res = await _mediator.Send(query, cancellationToken);
            return Ok(res);
        }
    }
}
