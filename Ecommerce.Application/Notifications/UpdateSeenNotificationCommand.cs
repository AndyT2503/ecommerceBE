using Ecommerce.Domain;
using Ecommerce.Infrastructure.Exceptions;
using Ecommerce.Infrastructure.User;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.Notifications
{
    internal class UpdateSeenNotificationHandler : IRequestHandler<UpdateSeenNotificationCommand, Unit>
    {
        private readonly MainDbContext _mainDbContext;
        private readonly ICurrentUser _currentUser;
        public UpdateSeenNotificationHandler(MainDbContext mainDbContext, ICurrentUser currentUser)
        {
            _mainDbContext = mainDbContext;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(UpdateSeenNotificationCommand request, CancellationToken cancellationToken)
        {
            var userRole = _currentUser.Role;
            var userId = _currentUser.Id;
            var notification = await _mainDbContext.Notifications
                                .Where(x => x.Id == request.notificationId)
                                .Where(x => x.UserId == userId || (x.UserId == null && x.GroupName == userRole) || (x.UserId == null && String.IsNullOrEmpty(userRole)))
                                .FirstOrDefaultAsync(cancellationToken);
            if (notification is null)
            {
                throw new CoreException("Không tìm thấy thông báo");
            }
            notification.Seen = true;
            await _mainDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
    public record UpdateSeenNotificationCommand(Guid notificationId) : IRequest<Unit>;

}
