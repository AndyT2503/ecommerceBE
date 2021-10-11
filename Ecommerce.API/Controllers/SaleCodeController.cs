using Ecommerce.Application.SaleCodes;
using Ecommerce.Domain.Const;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.API.Controllers
{
    [Route("api/sale-code")]
    [ApiController]
    public class SaleCodeController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SaleCodeController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Roles = AppRole.SuperAdmin)]
        [HttpPost]
        public async Task<IActionResult> CreateSaleCode(CreateSaleCodeCommand command, CancellationToken cancellationToken)
        {
            var dto = await _mediator.Send(command, cancellationToken);
            return Ok(dto);
        }

        [HttpGet()]
        public async Task<IActionResult> GetSaleCodes([FromQuery] GetSaleCodeQuery query, CancellationToken cancellationToken)
        {
            var res = await _mediator.Send(query, cancellationToken);
            return Ok(res);
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateSaleCode(UpdateSaleCodeCommand command, CancellationToken cancellationToken)
        {
            var res = await _mediator.Send(command, cancellationToken);
            return Ok(res);
        }

        [HttpDelete("{code}")]
        public async Task<IActionResult> DeleteSupplier(string code, CancellationToken cancellationToken)
        {
            var dto = await _mediator.Send(new DeleteSaleCodeCommand(code), cancellationToken);
            return Ok(dto);
        }
    }
}
