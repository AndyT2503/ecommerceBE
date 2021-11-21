using Ecommerce.Application.Orders.Dto;
using Ecommerce.Domain;
using Ecommerce.Domain.Model;
using Ecommerce.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.Orders
{
    public class CustomerGetOrderDetailQuery : IRequest<OrderDto>
    {
        public string PhoneNumber { get; init; }
        public string OrderCode { get; init; }
    }

    internal class CustomerGetOrderDetailHandler : IRequestHandler<CustomerGetOrderDetailQuery, OrderDto>
    {
        private readonly MainDbContext _mainDbContext;
        public CustomerGetOrderDetailHandler(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public async Task<OrderDto> Handle(CustomerGetOrderDetailQuery request, CancellationToken cancellationToken)
        {
            var order = await _mainDbContext.Orders.AsNoTracking()
                .Where(x => x.PhoneNumber == request.PhoneNumber && x.OrderCode == request.OrderCode)
                .Include(x => x.OrderDetails).ThenInclude(x => x.Category).ThenInclude(x => x.Product)
                .Include(x => x.Sale)
                .Include(x => x.OrderLogs)
                .FirstOrDefaultAsync(cancellationToken);

            if (order == null)
            {
                throw new CoreException("Order không tồn tại.");
            }

            var estimatedDelivery = order.CreatedAt.Date.AddDays(1);
            var priceSale = 0M;
            if (!string.IsNullOrEmpty(order.SaleCode))
            {
                priceSale = GetSalePrice(order.Sale, GetTotalPrice(order.OrderDetails));
            }
            var orderTracking = new OrderDto
            {
                Id = order.Id,
                OrderCode = order.OrderCode,
                Status = order.Status,
                Address = order.Address,
                CustomerName = order.CustomerName,
                PhoneNumber = order.PhoneNumber,
                ProvinceCode = order.ProvinceCode,
                DistrictCode = order.DistrictCode,
                PaymentMethod = order.PaymentMethod,
                PaymentStatus = order.PaymentStatus,
                EstimatedDeliveryAt = estimatedDelivery,
                SaleCode = order.SaleCode,
                PriceSale = priceSale,
                TotalPrice = order.Price,
                CreatedAt = order.CreatedAt,
                OrderDetails = order.OrderDetails.Select(x => new OrderDetailDto
                {
                    Id = x.Id, 
                    Price = x.Price, 
                    Quantity = x.Quantity,
                    Name = $"{x.Category.Product.Name} - {x.Category.Name}",
                    Image = x.Category.Image
                }) ,
                OrderLogs = order.OrderLogs.Select(x => new OrderLogDto
                {
                    Status = x.Status,
                    TimeStamp = x.Timestamp
                })
            };
            return orderTracking;
        }
        
        private static decimal GetSalePrice(SaleCode saleCode, decimal totalPrice)
        {
            if (saleCode.Percent * totalPrice / 100 > saleCode.MaxPrice)
            {
                return saleCode.MaxPrice;
            }

            return saleCode.Percent * totalPrice / 100;
        }

        private static decimal GetTotalPrice(ICollection<OrderDetail> orderDetails)
        {
            return orderDetails.Sum(x => x.Price * x.Quantity);
        }
    }
}

