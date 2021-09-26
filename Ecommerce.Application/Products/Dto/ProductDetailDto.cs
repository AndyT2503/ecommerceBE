using System;
using System.Collections.Generic;

namespace Ecommerce.Application.Products.Dto
{
    public class ProductDetailDto : ProductDto
    {
        public object Configuration { get; set; }
        public Guid SupplierId { get; init; }
        public Guid ProductTypeId { get; init; }
        public IEnumerable<ProductCategoryDto> Categories { get; init; }
    }
}
