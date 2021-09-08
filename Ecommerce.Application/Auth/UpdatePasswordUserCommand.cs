using Ecommerce.Domain;
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
    internal class UpdatePasswordUserHandler : IRequestHandler<UpdatePasswordUserCommand, Unit>
    {
        private readonly MainDbContext _mainDbContext;
        public UpdatePasswordUserHandler(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public async Task<Unit> Handle(UpdatePasswordUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _mainDbContext.Users.FirstOrDefaultAsync(x => x.Id == request.id, cancellationToken);
            if (user is null)
            {
                throw new CoreException("User không tồn tại");
            }
            user.Password = request.dto.NewPassword;
            await _mainDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
    public record UpdatePasswordUserCommand(Guid id, UserPasswordDto dto): IRequest<Unit>
    {
    }

    public class UserPasswordDto
    {
        public string NewPassword { get; init; }
    }
}
