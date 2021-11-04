using System;

namespace Ecommerce.Application.Products.Dto
{
    public class ProductCategoryDto
    {
        public Guid? Id { get; init; }
        public string Name { get; init; }
        public string Image { get; init; }
        public decimal Price { get; init; }
        public bool IsActive { get; init; }
        public string ProductName { get; init; }
    }
}
