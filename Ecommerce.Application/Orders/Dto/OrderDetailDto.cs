using System;

namespace Ecommerce.Application.Orders.Dto
{
    public class OrderDetailDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public decimal Price { get; init; }
        public decimal Quantity { get; init; }
        public string Image { get; init; }
    }
}
