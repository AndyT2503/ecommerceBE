using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.Socket.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly ILogger _logger;
        public NotificationHub(ILogger<NotificationHub> logger)
        {
            _logger = logger;
        }
        public override Task OnConnectedAsync()
        {
            _logger.LogInformation("Socket connected...");
            return base.OnConnectedAsync();
        }
    }
}
