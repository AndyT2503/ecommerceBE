using Ecommerce.Application.Orders.Dto;
using Ecommerce.Domain;
using Ecommerce.Domain.Model;
using Ecommerce.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.Orders
{
    public class GetOrderTrackingQuery : IRequest<OrderDto>
    {
        public string PhoneNumber { get; init; }
        public string OrderCode { get; init; }
    }

    internal class GetOrderTrackingHandler : IRequestHandler<GetOrderTrackingQuery, OrderDto>
    {
        private readonly MainDbContext _mainDbContext;
        public GetOrderTrackingHandler(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public async Task<OrderDto> Handle(GetOrderTrackingQuery request, CancellationToken cancellationToken)
        {
            var order = await _mainDbContext.Orders.AsNoTracking()
                .Where(x => x.PhoneNumber == request.PhoneNumber && x.OrderCode == request.OrderCode)
                .Include(x => x.OrderDetails)
                .Include(x => x.Sale)
                .FirstOrDefaultAsync(cancellationToken);

            if (order == null)
            {
                throw new CoreException("Order tracking không tồn tại.");
            }

            var estimatedDelivery = order.CreatedAt.Date.AddDays(1);
            var priceSale = GetSalePrice(order.Sale, GetTotalPrice(order.OrderDetails));

            var orderTracking = new OrderDto
            {
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
                OrderDetails = order.OrderDetails.Select(x => new OrderDetailDto { Id = x.Id, Price = x.Price, Quantity = x.Quantity })   
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

