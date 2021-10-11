using Ecommerce.Application.Notifications.Dto;
using Ecommerce.Domain;
using Ecommerce.Infrastructure.Common;
using Ecommerce.Infrastructure.LinQ;
using Ecommerce.Infrastructure.User;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.Notifications
{
    internal class GetNotificationHandler : IRequestHandler<GetNotificationQuery, PagingModel<NotificationDto>>
    {
        private readonly MainDbContext _mainDbContext;
        private readonly ICurrentUser _currentUser;
        public GetNotificationHandler(MainDbContext mainDbContext, ICurrentUser currentUser)
        {
            _mainDbContext = mainDbContext;
            _currentUser = currentUser;
        }

        public async Task<PagingModel<NotificationDto>> Handle(GetNotificationQuery request, CancellationToken cancellationToken)
        {
            var userRole = _currentUser.Role;
            var userId = _currentUser.Id;
            var query = _mainDbContext.Notifications
                                .AsNoTracking()
                                .Where(x => x.UserId == userId || (x.UserId == null && x.GroupName == userRole) || (x.UserId == null && String.IsNullOrEmpty(userRole)))
                                .OrderByDescending(x => x.CreatedAt);
            var notifications = await query.Select(x => new NotificationDto
            {
                Id = x.Id,
                GroupName = x.GroupName,
                UserId = x.UserId,
                EventName = x.EventName,
                Seen = x.Seen,
                MetaData = x.MetaData
            }).Page(request.PageIndex, request.PageSize).ToListAsync(cancellationToken);
            foreach (var item in notifications)
            {
                item.MetaData = JsonConvert.DeserializeObject<ExpandoObject>(item.MetaData.ToString(), new ExpandoObjectConverter());
            }
            var totalCount = await query.CountAsync(cancellationToken);
            return new PagingModel<NotificationDto>(notifications, totalCount, request.PageIndex, request.PageSize);
        }
    }
    public class GetNotificationQuery : IRequest<PagingModel<NotificationDto>>
    {
        public int PageIndex { get; init; }
        public int PageSize { get; init; }
    }
}
