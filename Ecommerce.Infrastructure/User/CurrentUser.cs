using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace Ecommerce.Infrastructure.User
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        protected ClaimsPrincipal ClaimsPrincipal => _httpContextAccessor.HttpContext?.User;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid Id => ClaimsPrincipal.FindFirst(ClaimTypes.NameIdentifier) != null ? new Guid(ClaimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value) : Guid.Empty;

        public string FullName => ClaimsPrincipal.FindFirst(ClaimTypes.Name).Value;

        public string Role => ClaimsPrincipal.FindFirst(ClaimTypes.Role).Value;
    }
}
