using Ecommerce.Application.Orders.Dto;
using Ecommerce.Application.Services.MailNotifyService;
using Ecommerce.Domain;
using Ecommerce.Domain.Const;
using Ecommerce.Domain.Helper;
using Ecommerce.Domain.Model;
using Ecommerce.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var order = new Order();
            order.OrderCode = NanoIdHelper.GenerateNanoId();
            order.Email = request.Email;
            order.PhoneNumber = request.PhoneNumber;
            order.ProvinceCode = request.ProvinceCode;
            order.DistrictCode = request.DistrictCode;
            order.Address = request.Address;
            order.Note = request.Note;
            order.CustomerName = request.CustomerName;
            if (!string.IsNullOrEmpty(request.SaleCode))
            {
                order.SaleCode = request.SaleCode;
            }
            order.PaymentMethod = request.PaymentMethod;
            order.PaymentStatus = request.PaymentMethod == PaymentMethod.Cash ? PaymentStatus.Waiting : PaymentStatus.Complete;
            order.Status = request.Status;
            foreach (var item in request.OrderDetails)
            {
               var category = await _mainDbContext.Categories.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == item.CategoryId && x.IsActive, cancellationToken);
               if (category is null)
               {
                   throw new CoreException("mặt hàng không tồn tại");
               }
              order.OrderDetails.Add(new OrderDetail() { CategoryId = item.CategoryId, Price = category.Price, Quantity = item.Quantity });
            }
            order.Price = await GetTotalPrice(order.OrderDetails, order.SaleCode, cancellationToken);
            await _mainDbContext.SaveChangesAsync(cancellationToken);
            await SentMail(order);
            return Unit.Value;

        }

        private async Task<decimal> GetTotalPrice(ICollection<OrderDetail> orderDetails, string saleCode,
            CancellationToken cancellationToken)
        {
            decimal totalPrice = orderDetails.Sum(item => item.Price * item.Quantity);
            if (String.IsNullOrEmpty(saleCode))
            {
                return totalPrice;
            }
            var code = await _mainDbContext.SaleCodes.AsNoTracking().FirstOrDefaultAsync(x => x.Code == saleCode, cancellationToken);
            if (code is null)
            {
                throw new CoreException("mã giảm giá không hợp lệ ");
            }

            if (code.ValidUntil.Date < DateTime.Now.Date)
            {
                throw new CoreException("mã giảm giá không hợp lệ ");
            }


            if (code.Percent * totalPrice / 100 > code.MaxPrice)
            {
                return totalPrice - code.MaxPrice;
            }

            return totalPrice - code.Percent * totalPrice / 100;
        }

        private async Task SentMail(Order order)
        {
            var orderDetailList = new List<object>();
            foreach (var item in order.OrderDetails)
            {
                var orderDetail = new
                {
                    categoryName = item.Category.Name,
                    quantity = item.Quantity,
                    price = item.Price
                };
                orderDetailList.Add(orderDetail);
            }

            var data = new
            {
                name = order.CustomerName,
                code = order.OrderCode,
                time = order.CreatedAt.ToString("dd/MM/yyyy HH:mm"),
                email = order.Email,
                phone = order.PhoneNumber,
                address = order.Address,
                payment = order.PaymentMethod,
                delivery = order.CreatedAt.AddDays(1).ToString("dd/MM/yyyy HH:mm"),
                url = " ", //TODO: Update url 
                saleCode = order.SaleCode,
                orderdetail = orderDetailList
            };
            await _mailNotifyService.SendMailAsync("bjnguyen97@gmail.com", data, "create_order");
        }
    }
    public class CreateOrderCommand : IRequest<Unit>
    {
        public string Email { get; init; }
        public string PhoneNumber { get; init; }
        public string ProvinceCode { get; init; }
        public string DistrictCode { get; init; }
        public string Address { get; init; }
        public string Note { get; init; }
        public string CustomerName { get; init; }
        public string SaleCode { get; init; }
        public string PaymentMethod { get; init; }
        public string Status { get; init; }
        public virtual ICollection<CreateOrderDetailDto> OrderDetails { get; init; }
    }
}
