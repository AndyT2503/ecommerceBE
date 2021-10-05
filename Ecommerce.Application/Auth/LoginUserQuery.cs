using Ecommerce.Application.Auth.Dto;
using Ecommerce.Application.Services.AuthService;
using Ecommerce.Domain;
using Ecommerce.Domain.Model;
using Ecommerce.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.Auth
{
    internal class LoginUserHandler : IRequestHandler<LoginUserQuery, UserLoginDto>
    {
        private readonly MainDbContext _mainDbContext;
        private readonly IConfiguration _configuration;
        private readonly AuthService _authService;
        public LoginUserHandler(MainDbContext mainDbContext, IConfiguration configuration, AuthService authService)
        {
            _mainDbContext = mainDbContext;
            _configuration = configuration;
            _authService = authService;
        }

        public async Task<UserLoginDto> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {

            var user = await _mainDbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Username == request.Username, cancellationToken);
            if (user is null)
            {
                throw new CoreException("Username is incorrect");
            }
            // Check passWord
            bool verified = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            if (!verified)
            {
                throw new CoreException("Password is incorrect");
            }
            var tokenString = _authService.GenerateToken(user);
            return new UserLoginDto()
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AccessToken = tokenString,
                Role = user.Role
            };
        }
    }
    public class LoginUserQuery : IRequest<UserLoginDto>
    {
        public string Username { get; init; }
        public string Password { get; init; }
    }
}
