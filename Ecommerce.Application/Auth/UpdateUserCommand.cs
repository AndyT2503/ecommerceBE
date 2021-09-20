using Ecommerce.Domain;
using Ecommerce.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.Auth
{
    internal class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly MainDbContext _mainDbContext;
        public UpdateUserHandler(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _mainDbContext.Users.FirstOrDefaultAsync(x => x.Id == request.id, cancellationToken);
            if (user is null)
            {
                throw new CoreException("User không tồn tại");
            }
            user.Role = request.dto.Role;
            user.LastName = request.dto.LastName;
            user.FirstName = request.dto.FirstName;
            await _mainDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
    public record UpdateUserCommand(Guid id, UserUpdateDto dto) : IRequest<Unit>
    {
    }

    public class UserUpdateDto
    {
        public string Role { get; init; }
        public string LastName { get; init; }
        public string FirstName { get; init; }
    }
}
