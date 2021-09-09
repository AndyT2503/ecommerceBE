using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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

        public Guid Id => new Guid(ClaimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value);

        public string FullName => ClaimsPrincipal.FindFirst(ClaimTypes.Name).Value;

        public string Role => ClaimsPrincipal.FindFirst(ClaimTypes.Role).Value;
    }
}
