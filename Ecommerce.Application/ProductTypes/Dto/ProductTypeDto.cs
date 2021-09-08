using Ecommerce.Application.Suppliers.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
