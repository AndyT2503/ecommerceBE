using Ecommerce.Domain;
using Ecommerce.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.Products
{
    internal class DeleteProductHandler : IRequestHandler<DeleteProductCommand, Unit>
    {
        private readonly MainDbContext _mainDbContext;
        public DeleteProductHandler(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _mainDbContext.Products.FirstOrDefaultAsync(x => x.Id == request.id, cancellationToken);
            if (product is null)
            {
                throw new CoreException("Sản phẩm không tồn tại");
            }
            _mainDbContext.Products.Remove(product);
            await _mainDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
    public record DeleteProductCommand(Guid id): IRequest<Unit>;
}
