using Ecommerce.Application.Products.Dto;
using Ecommerce.Domain;
using Ecommerce.Infrastructure.Common;
using Ecommerce.Infrastructure.LinQ;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.Products
{
    internal class GetProductHandler : IRequestHandler<GetProductQuery, PagingModel<ProductDto>>
    {
        private readonly MainDbContext _mainDbContext;
        public GetProductHandler(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public async Task<PagingModel<ProductDto>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var query = _mainDbContext.Products.AsNoTracking()
                .Include(x => x.ProductType)
                .Include(x => x.Supplier)
                .Include(x => x.Categories)
                .Where(i => EF.Functions.ILike(i.Name, $"%{request.Name}%")).Select(x => new ProductDto
                {
                    Id = x.Id,
                    Slug = x.Slug,
                    AvailableStatus = x.AvailableStatus,
                    Description = x.Description,
                    Name = x.Name,
                    OriginalPrice = x.OriginalPrice,
                    ProductTypeName = x.ProductType.Name,
                    SpecialFeatures = x.SpecialFeatures,
                    Status = x.Status,
                    SupplierName = x.Supplier.Name,
                    CurrentPrice = x.Categories.OrderBy(i => i.Price).First().Price,
                    Image = x.Categories.OrderBy(i => i.Price).First().Image
                });
            var totalCount = await query.CountAsync(cancellationToken);
            var products = new List<ProductDto>();
            if (request.PageIndex is not null && request.PageSize is not null)
            {
                products = await query.Page(request.PageIndex.Value, request.PageSize.Value).ToListAsync(cancellationToken);
            }
            else
            {
                products = await query.ToListAsync(cancellationToken);
            }
            return new PagingModel<ProductDto>(products, totalCount, request.PageIndex.GetValueOrDefault(), request.PageSize.GetValueOrDefault());
        }
    }
    public class GetProductQuery : IRequest<PagingModel<ProductDto>>
    {
        public int? PageSize { get; init; }
        public int? PageIndex { get; init; }
        public string Name { get; init; }
    }
}
