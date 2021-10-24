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
           foreach (var item in request.orderdetails)
            {
               var category = await _maindbcontext.categories.asnotracking()
                    .firstordefaultasync(x => x.id == item.categoryid && x.isactive, cancellationtoken: cancellationtoken);
               if (category is null)
                {
                   throw new coreexception("mặt hàng không tồn tại");
               }
              order.orderdetails.add(new orderdetail() { categoryid = item.categoryid, price = category.price, quantity = item.quantity });
          }

         order.price = await gettotalprice(order.orderdetails, order.salecode, cancellationtoken);


            var orderdetail = new orderdetails();
            orderdetail.categoryid = request.categoryid;
            orderdetail.quantity = request.quantity;
            orderdetail.price = request.price;

            datetime today = datetime.now.tostring("dddd , mmm dd yyyy,hh:mm:ss");
            datetime ship = today.adddays(3).tostring("dddd , mmm dd yyyy");

            var data = new
            {
                name = order.customername,
                code = order.ordercode,
                time = today,
                email = order.email,
                phone = order.phonenumber,
                address = order.address,
                payment = order.paymentmethod,
                delivery = ship,
                url = " ",
                orderdetail = new[]
                    {
                    categoryid = orderdetail.categoryid,
                    quantity = orderdetail.quantity,
                    price =  orderdetail.price,
                    salecodes= order.salecode,

                  }
            };
            await _mailnotifyservice.sendmailasync("bjnguyen97@gmail.com", data, "create_order");

            _mainDbContext.Orders.Add(order);
            await _mainDbContext.SaveChangesAsync(cancellationToken);
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
    }
    public class CreateOrderCommand : IRequest<Unit>
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ProvinceCode { get; set; }
        public string DistrictCode { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public string CustomerName { get; set; }
        public string SaleCode { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public virtual ICollection<CreateOrderDetailDto> OrderDetails { get; set; }
    }
}
