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
    internal class AddOrderDetailHandler : IRequestHandler<AddOrderDetailCommand, Unit>
    {
        private readonly MainDbContext _mainDbContext;

        public AddOrderDetailHandler( MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }
        public async Task<Unit> Handle(AddOrderDetailCommand request, CancellationToken cancellationToken)
        {
            var OrderDetail = new OrderDetail();
         //   OrderDetail.Order = request.Order;
         //   OrderDetail.Category = request.Category;
            OrderDetail.Price = request.Price;
            OrderDetail.Quantity = request.Quantity;
            OrderDetail.CategoryId = request.CategoryId;
            _mainDbContext.OrderDetails.Add(OrderDetail);
            await _mainDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
   public class AddOrderDetailCommand: IRequest<Unit>
    {
     //  public Guid OrderId { get; set; }
      //  public Order Order { get; set; }
        public Guid CategoryId { get; set; }
       // public Category Category { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
