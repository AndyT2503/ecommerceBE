using Ecommerce.Application.Ratings;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RatingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateProductRating(CreateRatingCommand command, CancellationToken cancellationToken)
        {
            var dto = await _mediator.Send(command, cancellationToken);
            return Ok(dto);
        }
    }
}
