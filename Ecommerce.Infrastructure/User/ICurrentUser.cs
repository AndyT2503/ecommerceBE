using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.User
{
    public interface ICurrentUser
    {
        Guid Id { get; }
        string Role { get; }
        string FullName { get; }
    }
}
