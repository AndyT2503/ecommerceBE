using System;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ecommerce.Application.Notifications.Dto;
using Ecommerce.Domain;
using Ecommerce.Infrastructure.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application.Notifications
{
    internal class GetNotificationUnSeenHandler : IRequestHandler<GetNumberNewNotificationQuery, NumberNewNotificationDto>
    {
        private readonly MainDbContext _mainDbContext;
        private readonly ICurrentUser _currentUser;
        public GetNotificationUnSeenHandler(MainDbContext mainDbContext, ICurrentUser currentUser)
        {
            _mainDbContext = mainDbContext;
            _currentUser = currentUser;
        }

        public async Task<NumberNewNotificationDto> Handle(GetNumberNewNotificationQuery request, CancellationToken cancellationToken)
        {
            var userRole = _currentUser.Role;
            var userId = _currentUser.Id;
            var numberNotification = await _mainDbContext.Notifications
                .AsNoTracking()
                .Where(x => x.UserId == userId || (x.UserId == null && x.GroupName == userRole) ||
                            (x.UserId == null && String.IsNullOrEmpty(userRole)))
                .Where(x => !x.Seen).CountAsync(cancellationToken);
            return new NumberNewNotificationDto {NumberNotification = numberNotification};
        }
    }
    public class GetNumberNewNotificationQuery : IRequest<NumberNewNotificationDto>
    {
    }
}