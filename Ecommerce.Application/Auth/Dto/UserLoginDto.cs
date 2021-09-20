namespace Ecommerce.Application.Auth.Dto
{
    class UserLoginDto : UserDto
    {
        public string AccessToken { get; init; }
    }
}
