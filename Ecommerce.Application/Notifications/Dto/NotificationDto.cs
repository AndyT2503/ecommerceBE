using System;

namespace Ecommerce.Application.Notifications.Dto
{
    public class NotificationDto
    {
        public Guid Id { get; init; }
        public string GroupName { get; init; }
        public Guid? UserId { get; init; }
        public bool Seen { get; init; }
        public object MetaData { get; set; }
        public string EventName { get; init; }
    }
}
