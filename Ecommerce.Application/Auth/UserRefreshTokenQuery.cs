using Ecommerce.Application.Auth.Dto;
using Ecommerce.Application.Services.AuthService;
using Ecommerce.Domain;
using Ecommerce.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.Auth
{
    internal class UserRefreshTokenHandler : IRequestHandler<UserRefreshTokenQuery, UserLoginDto>
    {
        private readonly MainDbContext _mainDbContext;
        private readonly AuthService _authService;
        public UserRefreshTokenHandler(MainDbContext mainDbContext, AuthService authService)
        {
            _mainDbContext = mainDbContext;
            _authService = authService;
        }

        public async Task<UserLoginDto> Handle(UserRefreshTokenQuery request, CancellationToken cancellationToken)
        {
            var userId = await _authService.ValidateRefreshToken(request.RefreshToken);
            var user = await _mainDbContext.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
            if (user is null)
            {
                throw new CoreException("User not found");
            }
            var tokenString = _authService.GenerateToken(user);
            return new UserLoginDto()
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AccessToken = tokenString,
                Role = user.Role,
                RefreshToken = request.RefreshToken
            };
        }
    }
    public class UserRefreshTokenQuery : IRequest<UserLoginDto>
    {
        public string RefreshToken { get; init; }
    }
}
