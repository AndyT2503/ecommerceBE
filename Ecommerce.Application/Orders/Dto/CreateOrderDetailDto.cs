using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Orders.Dto
{
    public class CreateOrderDetailDto
    {
        public Guid CategoryId { get; set; }
        public int Quantity { get; set; }
    }
}
