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

namespace Ecommerce.Application.Suppliers
{
    public class GetAllSupplierQuery : IRequest<IEnumerable<SupplierDto>>
    {
        public string Name { get; init; }
    }
    internal class GetAllSupplierHandler : IRequestHandler<GetAllSupplierQuery, IEnumerable<SupplierDto>>
    {
        private readonly MainDbContext _mainDbContext;
        public GetAllSupplierHandler(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public async Task<IEnumerable<SupplierDto>> Handle(GetAllSupplierQuery request, CancellationToken cancellationToken)
        {
            var suppliers = await _mainDbContext.Suppliers.AsNoTracking()
                .WhereIf(!string.IsNullOrEmpty(request.Name), i => EF.Functions.ILike(i.Name, $"%{request.Name}%"))
                .Select(x => new SupplierDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    Logo = x.Logo
                })
                .ToListAsync(cancellationToken);
            return suppliers;
        }
    }
}
