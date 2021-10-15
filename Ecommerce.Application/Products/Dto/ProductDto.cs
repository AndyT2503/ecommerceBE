using System;
using System.Collections.Generic;

namespace Ecommerce.Application.Products.Dto
{
    public class ProductDto
    {
        public Guid Id { get; init; }
        public string Slug { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public string Status { get; init; }
        public string AvailableStatus { get; init; }
        public decimal OriginalPrice { get; init; }
        public IEnumerable<string> SpecialFeatures { get; init; }
        public string SupplierName { get; init; }
        public string ProductTypeName { get; init; }
        public string Image { get; init; }
        public decimal? CurrentPrice { get; init; }

        public float MediumRate { get; init; }

        public int CountRate { get; init; }
    }
}
