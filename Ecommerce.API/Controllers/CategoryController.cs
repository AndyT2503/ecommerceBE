using Ecommerce.Application.Categories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryById(Guid id, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new DeleteCategoryByIdCommand(id), cancellationToken);
            return Ok(response);
        }
    }
}
