using Ecommerce.Application.Services.Socket.SocketService;
using Ecommerce.Domain;
using Ecommerce.Domain.Const;
using Ecommerce.Domain.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.NotificationService
{
    public class NotificationService
    {
        private readonly SocketService _socketService;
        private readonly MainDbContext _mainDbContext;
        public NotificationService(SocketService socketService, MainDbContext mainDbContext)
        {
            _socketService = socketService;
            _mainDbContext = mainDbContext;
        }

        public async Task SentNotificationByUserAsync(Guid userId, object message, string eventName, CancellationToken cancellationToken)
        {
            var newNotification = new Notification()
            {
                EventName = eventName,
                UserId = userId,
                MetaData = message,
            };
            _mainDbContext.Notifications.Add(newNotification);
            await _mainDbContext.SaveChangesAsync(cancellationToken);
            await _socketService.SendMessageByUserAsync(SocketEvent.NewNotification, userId.ToString(), message, cancellationToken);
        }

        public async Task SentNotificationByGroupAsync(string userRole, object message, string eventName, CancellationToken cancellationToken)
        {
            var newNotification = new Notification()
            {
                EventName = eventName,
                GroupName = userRole,
                MetaData = message,
            };
            _mainDbContext.Notifications.Add(newNotification);
            await _mainDbContext.SaveChangesAsync(cancellationToken);
            await _socketService.SendMessageByGroupAsync(SocketEvent.NewNotification, userRole, message, cancellationToken);
        }

        public async Task SentNotificationAsync(object message, string eventName, CancellationToken cancellationToken)
        {
            var newNotification = new Notification()
            {
                EventName = eventName,
                MetaData = message,
            };
            _mainDbContext.Notifications.Add(newNotification);
            await _mainDbContext.SaveChangesAsync(cancellationToken);
            await _socketService.SendMessageAsync(SocketEvent.NewNotification, message, cancellationToken);
        }

    }
}
