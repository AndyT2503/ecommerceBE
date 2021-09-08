using Ecommerce.Application.Auth.Dto;
using Ecommerce.Domain;
using Ecommerce.Infrastructure.User;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.Auth
{
    internal class UserProfileHandler : IRequestHandler<UserProfileQuery, UserDto>
    {
        private readonly MainDbContext _mainDbContext;
        private readonly ICurrentUser _currentUser;
        public UserProfileHandler(MainDbContext mainDbContext, ICurrentUser currentUser)
        {
            _mainDbContext = mainDbContext;
            _currentUser = currentUser;
        }

        public async Task<UserDto> Handle(UserProfileQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;
            var user = await _mainDbContext.Users.AsNoTracking().Select(x => new UserDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Role = x.Role,
                Username = x.Username
            }).FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
            return user;
        }
    }
    public class UserProfileQuery : IRequest<UserDto>
    {
    }
}
