using Ecommerce.Application.ProductTypes.Dto;
using Ecommerce.Application.Suppliers.Dto;
using Ecommerce.Domain;
using Ecommerce.Infrastructure.LinQ;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.ProductTypes
{
    internal class GetProductTypeHandler : IRequestHandler<GetProductTypeQuery, IEnumerable<ProductTypeDto>>
    {
        private readonly MainDbContext _mainDbContext;
        public GetProductTypeHandler(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public async Task<IEnumerable<ProductTypeDto>> Handle(GetProductTypeQuery request, CancellationToken cancellationToken)
        {
            var productTypeList = await _mainDbContext.ProductTypes.AsNoTracking()
                .Include(x => x.SupplierProductTypes)
                .WhereIf(!string.IsNullOrEmpty(request.Name), i => EF.Functions.ILike(i.Name, $"%{request.Name}%"))
                .Select(x => new ProductTypeDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Suppliers = x.SupplierProductTypes.Select(v => new SupplierDto
                    {
                        Id = v.SupplierId,
                        Name = v.Supplier.Name,
                        Code = v.Supplier.Code
                    })
                })
                .ToListAsync(cancellationToken);
            return productTypeList;
        }
    }
    public class GetProductTypeQuery : IRequest<IEnumerable<ProductTypeDto>>
    {
        public string Name { get; init; }
    }
}
