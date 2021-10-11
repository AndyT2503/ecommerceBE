using System;

namespace Ecommerce.Application.SaleCodes.Dto
{
    public class SaleCodeDto
    {
        public string Code { get; init; }
        public int Percent { get; init; }
        public decimal MaxPrice { get; init; }
        public DateTime ValidUntil { get; init; }
    }

}
