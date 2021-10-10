using System;
using System.Collections.Generic;
using Ecommerce.Application.Services.MailNotifyService;
using Ecommerce.Domain;
using Ecommerce.Domain.Const;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Ecommerce.Domain.Model;
using Microsoft.EntityFrameworkCore;

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
            var Order = new Order();
            Order.OrderCode = request.OrderCode;
            Order.Email = request.Email;
            Order.PhoneNumber = request.PhoneNumber;
            Order.ProvinceCode = request.ProvinceCode;
            Order.DistrictCode = request.DistrictCode;
            Order.Address = request.Address;
            Order.Note = request.Note;
            Order.CustomerName = request.CustomerName;
            Order.SaleCode = request.SaleCode;
            Order.Price = request.Price;
            Order.PaymentMethod = request.PaymentMethod;
            Order.PaymentStatus = request.PaymentStatus;
            Order.Status = request.Status;
            _mainDbContext.Orders.Add(Order);
            foreach (var item in request.OrderDetails)
            {
                var orderDetails =
                    await _mainDbContext.OrderDetails.FirstOrDefaultAsync(x => x.Id == item, cancellationToken);
                if (orderDetails is null)
                {
                    continue;
                }

                var addOrderDetail = new OrderDetail()
                {
                    OrderId = Order.Id,
                    CategoryId = orderDetails.CategoryId,
                    Price = orderDetails.Price,
                    Quantity = orderDetails.Quantity,

                };
                _mainDbContext.OrderDetails.Add(orderDetails);

            }
            await _mainDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
    public class CreateOrderCommand : IRequest<Unit>
    {
        public string OrderCode { get; init; }
        public string Email { get; init; }
        public string PhoneNumber { get; init; }
        public string ProvinceCode { get; init; }
        public string DistrictCode { get; init; }
        public string Address { get; init; }
        public string Note { get; init; }
        public string CustomerName { get; init; }
        public string SaleCode { get; init; }
        public decimal Price { get; init; }
        public string PaymentMethod { get; init; }
        public string PaymentStatus { get; init; }
        public string Status { get; init; }
        public virtual ICollection<Guid> OrderDetails { get; init; }
    }
}
