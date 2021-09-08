using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Auth.Dto
{
    class UserLoginDto : UserDto
    {
        public string AccessToken { get; init; }
    }
}
