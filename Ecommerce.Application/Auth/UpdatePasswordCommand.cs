using Ecommerce.Domain;
using Ecommerce.Infrastructure.Exceptions;
using Ecommerce.Infrastructure.User;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.Auth
{
    public class UpdatePasswordCommand : IRequest<Unit>
    {
        public string CurrentPassword { get; init; }
        public string NewPassword { get; init; }
    }

    internal class UpdatePasswordHandler : IRequestHandler<UpdatePasswordCommand, Unit>
    {
        private readonly MainDbContext _mainDbContext;
        private readonly ICurrentUser _currentUser;
        public UpdatePasswordHandler(MainDbContext mainDbContext, ICurrentUser currentUser)
        {
            _mainDbContext = mainDbContext;
            _currentUser = currentUser;
        }
        public async Task<Unit> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            if (request.NewPassword.Equals("") || request.CurrentPassword.Equals(""))
            {
                throw new CoreException("Mật khẩu mới khôngg được để trống");
            }

            if (request.NewPassword == request.CurrentPassword)
            {
                throw new CoreException("Đây là mật khẩu hiện tại");
            }
            var userId = _currentUser.Id;
            var user = await _mainDbContext.Users.FirstOrDefaultAsync(x => x.Id.Equals(userId), cancellationToken);
            var verified = BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.Password);
            if (!verified)
            {
                throw new CoreException("Mật khẩu hiện tại không đúng");
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            _mainDbContext.Users.Update(user);
            await _mainDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}








