using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ecommerce.Domain;
using Ecommerce.Domain.Model;
using MediatR;

namespace Ecommerce.Application.Orders
{
    internal class AddOrderHandler : IRequestHandler<AddOrderCommand, Unit>
    {
        private readonly MainDbContext _mainDbContext;

        public AddOrderHandler(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }
        public async Task<Unit> Handle(AddOrderCommand request, CancellationToken cancellationToken)
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
            //Order.CreatedAt = DateTime.Now;
            //Order.ModifiedAt = DateTime.Now;

            _mainDbContext.Orders.Add(Order);
            await _mainDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }

    public class AddOrderCommand : IRequest<Unit>
    {
        public string OrderCode { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ProvinceCode { get; set; }
        public string DistrictCode { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public string CustomerName { get; set; }
        public string SaleCode { get; set; }
        public decimal Price { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public string Status { get; set; }
    }
}
