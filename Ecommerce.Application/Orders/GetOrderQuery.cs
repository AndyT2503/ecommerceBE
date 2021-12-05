using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ecommerce.Application.Orders.Dto;
using Ecommerce.Domain;
using Ecommerce.Infrastructure.Common;
using Ecommerce.Infrastructure.LinQ;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application.Orders
{
    internal class GetOrderHandler : IRequestHandler<GetOrderQuery, PagingModel<OrderDto>>
    {
        private readonly MainDbContext _mainDbContext;

        public GetOrderHandler(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public async Task<PagingModel<OrderDto>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var query = _mainDbContext.Orders.AsNoTracking()
                .WhereIf(!string.IsNullOrEmpty(request.Code), x => x.OrderCode.StartsWith(request.Code))
                .WhereIf(!string.IsNullOrEmpty(request.DistrictCode), x => x.DistrictCode == request.DistrictCode)
                .WhereIf(!string.IsNullOrEmpty(request.ProvinceCode), x => x.ProvinceCode == request.ProvinceCode)
                .WhereIf(!string.IsNullOrEmpty(request.Status), x => x.Status == request.Status)
                .WhereIf(!string.IsNullOrEmpty(request.PaymentStatus), x => x.PaymentStatus == request.PaymentStatus)
                .WhereIf(request.StartDate != null && request.EndDate != null,
                    x => request.StartDate <= x.CreatedAt && x.CreatedAt >= request.EndDate);
            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query.OrderBy(request.SortName ?? "CreatedAt", request.Ascend)
                .Page(request.PageIndex, request.PageSize).Select(x => new OrderDto
                {
                    Id = x.Id,
                    Address = x.Address,
                    Status = x.Status,
                    CreatedAt = x.CreatedAt,
                    CustomerName = x.CustomerName,
                    DistrictCode = x.DistrictCode,
                    OrderCode = x.OrderCode,
                    PaymentMethod = x.PaymentMethod,
                    PaymentStatus = x.PaymentStatus,
                    PhoneNumber = x.PhoneNumber,
                    TotalPrice = x.Price,
                    ProvinceCode = x.ProvinceCode
                }).ToListAsync(cancellationToken);
            return new PagingModel<OrderDto>(items, totalCount, request.PageIndex, request.PageSize);
        }
    }

    public class GetOrderQuery : IRequest<PagingModel<OrderDto>>
    {
        public int PageIndex { get; init; }
        public int PageSize { get; init; }
        public string Code { get; init; }
        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
        public string DistrictCode { get; init; }
        public string ProvinceCode { get; init; }
        public string Status { get; init; }
        public string PaymentStatus { get; init; }
        public string SortName { get; init; }
        public bool Ascend { get; init; }
    }
}
