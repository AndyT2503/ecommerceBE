using Ecommerce.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.Socket.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly ILogger _logger;
        private readonly MainDbContext _mainDbContext;
        public NotificationHub(ILogger<NotificationHub> logger, MainDbContext mainDbContext)
        {
            _logger = logger;
            _mainDbContext = mainDbContext;
        }
        public override async Task OnConnectedAsync()
        {
            var userId = new Guid(Context.UserIdentifier);
            var user = await _mainDbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user is not null)
            {
                var groupName = user.Role;
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                _logger.LogInformation($"{userId} connected...");
            }
        }
    }
}
