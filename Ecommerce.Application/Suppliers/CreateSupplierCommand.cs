using Ecommerce.Domain;
using Ecommerce.Domain.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.Suppliers
{
    internal class CreateSupplierHandler : IRequestHandler<CreateSupplierCommand, Unit>
    {
        private readonly MainDbContext _mainDbContext;
        public CreateSupplierHandler(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public async Task<Unit> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
        {
            var newSupplier = new Supplier() { Name = request.Name, Logo = request.Logo, Code = request.Code };
            _mainDbContext.Suppliers.Add(newSupplier);
            foreach (var item in request.ProductTypes)
            {
                var productType = await _mainDbContext.ProductTypes.FirstOrDefaultAsync(x => x.Id == item, cancellationToken);
                if (productType is null)
                {
                    continue;
                }
                var supplierProductType = new SupplierProductType() { ProductType = productType, Supplier = newSupplier };
                _mainDbContext.SupplierProductTypes.Add(supplierProductType);
            }
            await _mainDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
    public class CreateSupplierCommand : IRequest<Unit>
    {
        public string Name { get; init; }
        public string Logo { get; init; }
        public string Code { get; init; }
        public IEnumerable<Guid> ProductTypes { get; init; }
    }
}
