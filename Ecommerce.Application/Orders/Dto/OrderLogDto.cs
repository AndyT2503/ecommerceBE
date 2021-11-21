using System;

namespace Ecommerce.Application.Orders.Dto
{
    public class OrderLogDto
    {
        public string Status { get; init; }
        public DateTime TimeStamp { get; init; }
    }
}