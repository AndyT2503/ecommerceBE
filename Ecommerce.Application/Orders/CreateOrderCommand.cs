using System;
using System.Collections.Generic;
using Ecommerce.Application.Services.MailNotifyService;
using Ecommerce.Domain;
using Ecommerce.Domain.Const;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Ecommerce.Application.Orders.Dto;
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
            var order = new Order();
            order.OrderCode = request.OrderCode;
            order.Email = request.Email;
            order.PhoneNumber = request.PhoneNumber;
            order.ProvinceCode = request.ProvinceCode;
            order.DistrictCode = request.DistrictCode;
            order.Address = request.Address;
            order.Note = request.Note;
            order.CustomerName = request.CustomerName;
            order.SaleCode = request.SaleCode;
            order.Price = request.Price;
            order.PaymentMethod = request.PaymentMethod;
            order.PaymentStatus = request.PaymentStatus;
            order.Status = request.Status;
            foreach (var item in request.OrderDetails)
            {
                order.OrderDetails.Add(new OrderDetail() { CategoryId = item.CategoryId, Price = item.Price, Quantity = item.Quantity });
            }
            _mainDbContext.Orders.Add(order);
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
        public virtual ICollection<OrderDto> OrderDetails { get; init; }
    }
}
