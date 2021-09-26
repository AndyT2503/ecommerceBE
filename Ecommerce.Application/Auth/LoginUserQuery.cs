using Ecommerce.Application.Auth.Dto;
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
        public LoginUserHandler(MainDbContext mainDbContext, IConfiguration configuration)
        {
            _mainDbContext = mainDbContext;
            _configuration = configuration;
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
            var tokenString = GenerateToken(user);
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

        private string GenerateToken(User user)
        {
            var credential = _configuration["AppCredential"];
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(credential);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
    public class LoginUserQuery : IRequest<UserLoginDto>
    {
        public string Username { get; init; }
        public string Password { get; init; }
    }
}
