using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Auth.Dto
{
    public class UserDto
    {
        public Guid Id { get; init; }
        public string Username { get; init; }
        public string Role { get; init; }
        public string LastName { get; init; }
        public string FirstName { get; init; }
    }
}
