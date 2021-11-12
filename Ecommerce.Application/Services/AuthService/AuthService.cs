using Ecommerce.Domain.Model;
using Ecommerce.Infrastructure.Exceptions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.AuthService
{
    public class AuthService
    {
        private const int Access_Token_LifeTime = 1;
        private const int Refresh_Token_LifeTime = 2;
        private readonly IConfiguration _configuration;
        private readonly IDistributedCache _cache;
        public AuthService(IConfiguration configuration, IDistributedCache cache)
        {
            _configuration = configuration;
            _cache = cache;
        }
        public string GenerateToken(User user)
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
                Expires = DateTime.UtcNow.AddMinutes(Access_Token_LifeTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        public async Task<string> GenerateRefreshToken(Guid userId)
        {
            var randomNumber = new byte[32];
            var refreshToken = "";
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                refreshToken = Convert.ToBase64String(randomNumber);
            }
            var userCache = new UserCacheDto { UserId = userId };
            var serializedUser = JsonConvert.SerializeObject(userCache);
            var cacheData = Encoding.UTF8.GetBytes(serializedUser);
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddHours(Refresh_Token_LifeTime)
            };
            await _cache.SetAsync(refreshToken, cacheData, cacheOptions);
            return refreshToken;
        }

        public async Task<Guid> ValidateRefreshToken(string refreshToken)
        {
            var redisUser = await _cache.GetAsync(refreshToken);
            if (redisUser is null)
            {
                throw new CoreException("Invalid Refresh Token");
            }
            var serializedUser = Encoding.UTF8.GetString(redisUser);
            var userCache = JsonConvert.DeserializeObject<UserCacheDto>(serializedUser);
            return userCache.UserId;
        }
    }
}
