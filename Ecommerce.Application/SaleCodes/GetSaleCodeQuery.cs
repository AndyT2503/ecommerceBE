using Ecommerce.Application.SaleCodes.Dto;
using Ecommerce.Domain;
using Ecommerce.Domain.Model;
using Ecommerce.Infrastructure.Common;
using Ecommerce.Infrastructure.LinQ;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.SaleCodes
{
    public class GetSaleCodeQuery : IRequest<PagingModel<SaleCodeDto>>
    {
        public int? PageIndex { get; init; }
        public int? PageSize { get; init; }
        public string Code { get; init; }
    }

    internal class GetSaleCodeHandler : IRequestHandler<GetSaleCodeQuery, PagingModel<SaleCodeDto>>
    {
        private readonly MainDbContext _mainDbContext;
        public GetSaleCodeHandler(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public async Task<PagingModel<SaleCodeDto>> Handle(GetSaleCodeQuery request, CancellationToken cancellationToken)
        {
            var query = _mainDbContext.SaleCodes.AsNoTracking()
                .WhereIf(!string.IsNullOrEmpty(request.Code), i => EF.Functions.ILike(i.Code, $"%{request.Code}%")).Select(x => new SaleCodeDto
                {
                    Code = x.Code,
                    Percent = x.Percent,
                    MaxPrice = x.MaxPrice,
                    ValidUntil = x.ValidUntil
                });
            var totalCount = await query.CountAsync(cancellationToken);
            var saleCodes = new List<SaleCodeDto>();
            if (request.PageIndex is not null && request.PageSize is not null)
            {
                saleCodes = await query.Page(request.PageIndex.Value, request.PageSize.Value).ToListAsync(cancellationToken);
            }
            else
            {
                saleCodes = await query.ToListAsync(cancellationToken);
            }
            return new PagingModel<SaleCodeDto>(saleCodes, totalCount, request.PageIndex.GetValueOrDefault(), request.PageSize.GetValueOrDefault());
        }
    }
}
