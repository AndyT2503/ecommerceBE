using Ecommerce.Domain;
using Ecommerce.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.ProductTypes
{
    internal class DeleteProductTypeHandler : IRequestHandler<DeleteProductTypeCommand, Unit>
    {
        private readonly MainDbContext _mainDbContext;
        public DeleteProductTypeHandler(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public async Task<Unit> Handle(DeleteProductTypeCommand request, CancellationToken cancellationToken)
        {
            var productType = await _mainDbContext.ProductTypes.FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);
            if (productType is null)
            {
                throw new CoreException("Item not Found");
            }
            _mainDbContext.ProductTypes.Remove(productType);
            await _mainDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
    public record DeleteProductTypeCommand(Guid Id) : IRequest<Unit>
    {
    }
}
