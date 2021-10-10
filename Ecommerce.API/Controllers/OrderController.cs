using Ecommerce.Application.Orders;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateOrder(CreateOrderCommand command, CancellationToken cancellationToken)
        //{
        //    var dto = await _mediator.Send(command, cancellationToken);
        //    return Ok(dto);
        //}

        [HttpPost("Order")]
        public async Task<IActionResult> CreateSupplier(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var dto = await _mediator.Send(command, cancellationToken);
            return Ok(dto);
        }
    }
}
