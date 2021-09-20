using Ecommerce.Domain;
using Ecommerce.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.Auth
{
    internal class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly MainDbContext _mainDbContext;
        public DeleteUserHandler(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _mainDbContext.Users.FirstOrDefaultAsync(x => x.Id == request.id, cancellationToken);
            if (user is null)
            {
                throw new CoreException("User không tồn tại");
            }
            _mainDbContext.Users.Remove(user);
            await _mainDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
    public record DeleteUserCommand(Guid id) : IRequest<Unit>
    {
    }
}
