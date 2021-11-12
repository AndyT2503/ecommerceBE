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
    internal class UserRefreshTokenHandler : IRequestHandler<UserRefreshTokenQuery, RefreshTokenDto>
    {
        private readonly MainDbContext _mainDbContext;
        private readonly AuthService _authService;
        public UserRefreshTokenHandler(MainDbContext mainDbContext, AuthService authService)
        {
            _mainDbContext = mainDbContext;
            _authService = authService;
        }

        public async Task<RefreshTokenDto> Handle(UserRefreshTokenQuery request, CancellationToken cancellationToken)
        {
            var userId = await _authService.ValidateRefreshToken(request.RefreshToken);
            var user = await _mainDbContext.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
            if (user is null)
            {
                throw new CoreException("User not found");
            }
            var tokenString = _authService.GenerateToken(user);
            return new RefreshTokenDto()
            {
                AccessToken = tokenString
            };
        }
    }
    public class UserRefreshTokenQuery : IRequest<RefreshTokenDto>
    {
        public string RefreshToken { get; init; }
    }
}
