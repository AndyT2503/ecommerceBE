using Ecommerce.Application.Auth;
using Ecommerce.Domain.Const;
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
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = AppRole.SuperAdmin)]
        [HttpPost("user")]
        public async Task<IActionResult> CreateUser(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var dto = await _mediator.Send(command, cancellationToken);
            return Ok(dto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginUserQuery query, CancellationToken cancellationToken)
        {
            var dto = await _mediator.Send(query, cancellationToken);
            return Ok(dto);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> UserRefreshToken(UserRefreshTokenQuery query, CancellationToken cancellationToken)
        {
            var dto = await _mediator.Send(query, cancellationToken);
            return Ok(dto);
        }

        [Authorize]
        [HttpGet("user-profile")]
        public async Task<IActionResult> GetUserProfile([FromQuery] UserProfileQuery query, CancellationToken cancellationToken)
        {
            var dto = await _mediator.Send(query, cancellationToken);
            return Ok(dto);
        }

        [Authorize(Roles = AppRole.SuperAdmin)]
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers([FromQuery] GetUsersQuery query, CancellationToken cancellationToken)
        {
            var dto = await _mediator.Send(query, cancellationToken);
            return Ok(dto);
        }

        [Authorize(Roles = AppRole.SuperAdmin)]
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(Guid id, CancellationToken cancellationToken)
        {
            var dto = await _mediator.Send(new DeleteUserCommand(id), cancellationToken);
            return Ok(dto);
        }

        [Authorize(Roles = AppRole.SuperAdmin)]
        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUserInfo(Guid id, [FromBody] UserUpdateDto body, CancellationToken cancellationToken)
        {
            var dto = await _mediator.Send(new UpdateUserCommand(id, body), cancellationToken);
            return Ok(dto);
        }

        [Authorize(Roles = AppRole.SuperAdmin)]
        [HttpPut("users/{id}/password")]
        public async Task<IActionResult> UpdatePasswordUser(Guid id, [FromBody] UserPasswordDto body, CancellationToken cancellationToken)
        {
            var dto = await _mediator.Send(new AdminUpdatePasswordUserCommand(id, body), cancellationToken);
            return Ok(dto);
        }
        [Authorize]
        [HttpPut("users/password")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordCommand command, CancellationToken cancellationToken)
        {
            var dto = await _mediator.Send(command, cancellationToken);
            return Ok(dto);
        }

    }
}
