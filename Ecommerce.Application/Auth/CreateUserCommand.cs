using Ecommerce.Domain;
using Ecommerce.Domain.Model;
using Ecommerce.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.Auth
{
    internal class CreateUserHandler : IRequestHandler<CreateUserCommand, Unit>
    {
        private readonly MainDbContext _mainDbContext;
        public CreateUserHandler(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var isValidateUser = await ValidateUser(request.Username, cancellationToken);
            if (!isValidateUser)
            {
                throw new CoreException("Username đã tồn tại");
            }
            var newUser = new User() { FirstName = request.FirstName, LastName = request.LastName, Role = request.Role, Username = request.Username, Password = request.Password };
            _mainDbContext.Users.Add(newUser);
            await _mainDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private async Task<bool> ValidateUser(string username, CancellationToken cancellationToken)
        {
            var user = await _mainDbContext.Users.FirstOrDefaultAsync(v => v.Username == username, cancellationToken);
            if (user is null)
            {
                return true;
            }
            return false;
        }
    }
    public class CreateUserCommand : IRequest<Unit>
    {
        public string Username { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Role { get; init; }
        public string Password { get; init; }
    }
}
