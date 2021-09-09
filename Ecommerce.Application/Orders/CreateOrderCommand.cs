using Ecommerce.Domain;
using Ecommerce.Infrastructure.MailNotify;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.Orders
{
    internal class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Unit>
    {
        private readonly MainDbContext _mainDbContext;
        private readonly IMailNotifyService _mailNotifyService;
        public CreateOrderHandler(MainDbContext mainDbContext, IMailNotifyService mailNotifyService)
        {
            _mainDbContext = mainDbContext;
            _mailNotifyService = mailNotifyService;
        }

        public async Task<Unit> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            //TODO: Implement Create Order
            var subject = "Bạn đã khởi tạo đơn hàng thành công";
            var body = "Test gửi email";
            await _mailNotifyService.SendMailAsync(request.Email, subject, body);
            return Unit.Value;
        }
    }
    public class CreateOrderCommand : IRequest<Unit>
    {
        public string Email { get; init; }
    }
}
