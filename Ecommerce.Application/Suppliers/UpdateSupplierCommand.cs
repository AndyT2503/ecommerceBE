using Ecommerce.Domain;
using Ecommerce.Domain.Model;
using Ecommerce.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.Suppliers
{
    internal class UpdateSupplierHandler : IRequestHandler<UpdateSupplierCommand, Unit>
    {
        private readonly MainDbContext _mainDbContext;
        public UpdateSupplierHandler(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public async Task<Unit> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = await _mainDbContext.Suppliers.Include(x => x.SupplierProductTypes).FirstOrDefaultAsync(v => v.Id == request.Id, cancellationToken);
            if (supplier is null)
            {
                throw new CoreException("Item not Found");
            }
            supplier.Logo = request.Logo;
            supplier.Name = request.Name;
            supplier.Code = request.Code;
            _mainDbContext.SupplierProductTypes.RemoveRange(supplier.SupplierProductTypes);
            foreach (var item in request.ProductTypes)
            {
                var productType = await _mainDbContext.ProductTypes.FirstOrDefaultAsync(x => x.Id == item, cancellationToken);
                if (productType is null)
                {
                    continue;
                }
                var supplierProductType = new SupplierProductType() { ProductType = productType, Supplier = supplier };
                _mainDbContext.SupplierProductTypes.Add(supplierProductType);
            }
            await _mainDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
    public class UpdateSupplierCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Name { get; init; }
        public string Logo { get; init; }
        public string Code { get; init; }
        public IEnumerable<Guid> ProductTypes { get; init; }
    }
}
