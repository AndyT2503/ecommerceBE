using System;

namespace Ecommerce.Application.Orders.Dto
{
    public class CreateOrderDetailDto
    {
        public Guid CategoryId { get; set; }
        public int Quantity { get; set; }
    }
}
