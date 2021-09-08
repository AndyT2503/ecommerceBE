using Ecommerce.Application.ProductTypes.Dto;
using System;
using System.Collections.Generic;

namespace Ecommerce.Application.Suppliers.Dto
{
    public class SupplierDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Code { get; init; }
        public string Logo { get; init; }
        public IEnumerable<ProductTypeDto> ProductTypes { get; init; }
    }
}
