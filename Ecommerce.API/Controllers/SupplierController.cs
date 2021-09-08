using Ecommerce.Application.Suppliers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SupplierController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateSupplier(CreateSupplierCommand command, CancellationToken cancellationToken)
        {
            var dto = await _mediator.Send(command, cancellationToken);
            return Ok(dto);
        }

        [HttpGet()]
        public async Task<IActionResult> GetSupplier([FromQuery] GetSupplierQuery query, CancellationToken cancellationToken)
        {
            var dto = await _mediator.Send(query, cancellationToken);
            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(Guid id, CancellationToken cancellationToken)
        {
            var dto = await _mediator.Send(new DeleteSupplierCommand(id), cancellationToken);
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplierById(Guid id, CancellationToken cancellationToken)
        {
            var dto = await _mediator.Send(new GetSupplierByIdQuery(id), cancellationToken);
            return Ok(dto);
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateSupplier(UpdateSupplierCommand command, CancellationToken cancellationToken)
        {
            var dto = await _mediator.Send(command, cancellationToken);
            return Ok(dto);
        }
    }
}
