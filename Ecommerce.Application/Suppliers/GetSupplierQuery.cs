using Ecommerce.Application.Suppliers.Dto;
using Ecommerce.Domain;
using Ecommerce.Infrastructure.Common;
using Ecommerce.Infrastructure.LinQ;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.Suppliers
{
    internal class GetSupplierHandler : IRequestHandler<GetSupplierQuery, PagingModel<SupplierDto>>
    {
        private readonly MainDbContext _mainDbContext;
        public GetSupplierHandler(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public async Task<PagingModel<SupplierDto>> Handle(GetSupplierQuery request, CancellationToken cancellationToken)
        {
            var query = _mainDbContext.Suppliers.AsNoTracking()
                .WhereIf(!string.IsNullOrEmpty(request.Name), i => EF.Functions.ILike(i.Name, $"%{request.Name}%")).Select(x => new SupplierDto
                {
                    Id = x.Id,
                    Logo = x.Logo,
                    Name = x.Name,
                    Code = x.Code
                });
            var totalCount = await query.CountAsync(cancellationToken);
            var suppliers = new List<SupplierDto>();
            if (request.PageIndex is not null && request.PageSize is not null)
            {
                suppliers = await query.Page(request.PageIndex.Value, request.PageSize.Value).ToListAsync(cancellationToken);
            }
            else
            {
                suppliers = await query.ToListAsync(cancellationToken);
            }
            return new PagingModel<SupplierDto>(suppliers, totalCount, request.PageIndex.GetValueOrDefault(), request.PageSize.GetValueOrDefault());
        }
    }
    public class GetSupplierQuery : IRequest<PagingModel<SupplierDto>>
    {
        public int? PageSize { get; init; }
        public int? PageIndex { get; init; }
        public string Name { get; init; }
    }
}
