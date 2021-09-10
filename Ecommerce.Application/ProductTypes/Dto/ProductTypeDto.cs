using Ecommerce.Application.Suppliers.Dto;
using System;
using System.Collections.Generic;

namespace Ecommerce.Application.ProductTypes.Dto
{
    public class ProductTypeDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Code { get; init; }
        public IEnumerable<SupplierDto> Suppliers { get; init; }
    }
}
