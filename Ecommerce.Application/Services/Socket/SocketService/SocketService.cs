using Ecommerce.Application.Services.Socket.Hubs;
using Ecommerce.Domain.Helper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
namespace Ecommerce.Application.Services.Socket.SocketService
{
    public class SocketService
    {
        private readonly IHubContext<NotificationHub> _hub;
        private readonly ILogger _logger;
        public SocketService(IHubContext<NotificationHub> hub, ILogger<SocketService> logger)
        {
            _hub = hub;
            _logger = logger;
        }

        public async Task SendMessageAsync(string eventName, object message, CancellationToken cancellationToken)
        {
            _logger.LogInformation(JsonHelper.ToJson(message, eventName, "sent socket event"));
            await _hub.Clients.All.SendAsync(eventName, message, cancellationToken);
        }

        public async Task SendMessageByUserAsync(string eventName, string userId, object message, CancellationToken cancellationToken)
        {
            _logger.LogInformation(JsonHelper.ToJson(message, eventName, $"sent socket event to {userId}"));
            await _hub.Clients.User(userId).SendAsync(eventName, message, cancellationToken);
        }

        public async Task SendMessageByGroupAsync(string eventName, string groupName, object message, CancellationToken cancellationToken)
        {
            _logger.LogInformation(JsonHelper.ToJson(message, eventName, $"sent socket event to {groupName}"));
            await _hub.Clients.Group(groupName).SendAsync(eventName, message, cancellationToken);
        }
    }
}
