using System;

namespace Ecommerce.Infrastructure.User
{
    public interface ICurrentUser
    {
        Guid Id { get; }
        string Role { get; }
        string FullName { get; }
    }
}
