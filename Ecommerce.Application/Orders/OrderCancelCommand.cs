using System;
using System.Threading;
using System.Threading.Tasks;
using Ecommerce.Domain;
using Ecommerce.Domain.Const;
using Ecommerce.Domain.Model;
using Ecommerce.Infrastructure.Exceptions;
using Ecommerce.Infrastructure.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application.Orders
{
    internal class OrderCancelHandle : IRequestHandler<OrderCancelCommand, Unit>
    {
        private readonly MainDbContext _mainDbContext;
        private readonly ICurrentUser _currentUser;
        public OrderCancelHandle(MainDbContext mainDbContext, ICurrentUser currentUser)
        {
            _currentUser = currentUser;
            _mainDbContext = mainDbContext;
        }

        public async Task<Unit> Handle(OrderCancelCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.Id;
            var order = await _mainDbContext.Orders.FirstOrDefaultAsync(x => x.Id == request.OrderId,
                cancellationToken);
            if (order is null)
            {
                throw new CoreException("Không tìm thấy đơn hàng");
            }
            if (order.Status == OrderStatus.Transporting || order.Status == OrderStatus.Complete)
            {
                throw new CoreException("Đơn hàng không thể hủy");
            }
            if (order.Status == OrderStatus.Cancel)
            {
                throw new CoreException("Đơn hàng đã bị hủy");
            }

            order.Status = OrderStatus.Cancel;
            order.OrderLogs.Add(new OrderLog{Status = OrderStatus.Cancel, Timestamp = DateTime.UtcNow, UserId = userId});
            await _mainDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
    public record OrderCancelCommand(Guid OrderId) : IRequest<Unit>;
}