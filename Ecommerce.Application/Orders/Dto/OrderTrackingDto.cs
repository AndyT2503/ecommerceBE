using System;
using System.Collections.Generic;

namespace Ecommerce.Application.Orders.Dto
{
    public class OrderTrackingDto
    {
        public string OrderCode { get; init; }
        public string Status { get; init; }
        public string Address { get; init; }
        public string CustomerName { get; init; }
        public string PhoneNumber { get; init; }
        public string ProvinceCode { get; init; }
        public string DistrictCode { get; init; }
        public string PaymentMethod { get; init; }
        public string PaymentStatus { get; init; }
        public DateTime EstimatedDeliveryAt { get; init; }
        public DateTime CreatedAt { get; init; }
        public string SaleCode { get; init; }
        public decimal PriceSale { get; init; }
        public decimal TotalPrice { get; init; }
        public IEnumerable<OrderDetailDto> OrderDetails { get; init; }
    }
}
