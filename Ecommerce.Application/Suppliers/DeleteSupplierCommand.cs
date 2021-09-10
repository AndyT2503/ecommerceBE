using Ecommerce.Domain;
using Ecommerce.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.Suppliers
{
    internal class DeleteSupplierHandler : IRequestHandler<DeleteSupplierCommand, Unit>
    {
        private readonly MainDbContext _mainDbContext;
        public DeleteSupplierHandler(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public async Task<Unit> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = await _mainDbContext.Suppliers.FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);
            if (supplier is null)
            {
                throw new CoreException("Item not Found");
            }
            _mainDbContext.Suppliers.Remove(supplier);
            await _mainDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
    public record DeleteSupplierCommand(Guid Id) : IRequest<Unit>
    {
    }
}
