using Ecommerce.Application.Products.Dto;
using Ecommerce.Domain;
using Ecommerce.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.Products
{
    internal class GetProductDetailHandler : IRequestHandler<GetProductDetailQuery, ProductDetailDto>
    {
        private readonly MainDbContext _mainDbContext;
        public GetProductDetailHandler(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public async Task<ProductDetailDto> Handle(GetProductDetailQuery request, CancellationToken cancellationToken)
        {
            var product = await _mainDbContext.Products.AsNoTracking()
                .Include(x => x.ProductType)
                .Include(x => x.Supplier)
                .Select(x => new ProductDetailDto
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
                    Configuration = x.Configuration,
                    ProductTypeId = x.ProductTypeId,
                    SupplierId = x.SupplierId,
                    Categories = x.Categories.Select(i => new ProductCategoryDto
                    {
                        Id = i.Id,
                        Image = i.Image,
                        Name = i.Name,
                        Price = i.Price
                    })
                }).FirstOrDefaultAsync(x => x.Slug == request.slug, cancellationToken);
            if (product is null)
            {
                throw new CoreException("Product is not found");
            }
            product.Configuration = JsonConvert.DeserializeObject<ExpandoObject[]>(product.Configuration.ToString(), new ExpandoObjectConverter());
            return product;
        }
    }
    public record GetProductDetailQuery(string slug) : IRequest<ProductDetailDto>;
}
