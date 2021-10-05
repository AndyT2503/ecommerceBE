using Ecommerce.Application.Services.MailNotifyService;
using Ecommerce.Domain;
using Ecommerce.Domain.Const;
using MediatR;
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
            var properties = new
            {
                Url = "test",
                Name = "Tu"
            };
            await _mailNotifyService.SendMailAsync(request.Email, properties, NotifyEvent.CreateOrder);
            return Unit.Value;
        }
    }
    public class CreateOrderCommand : IRequest<Unit>
    {
        public string Email { get; init; }
    }
}
