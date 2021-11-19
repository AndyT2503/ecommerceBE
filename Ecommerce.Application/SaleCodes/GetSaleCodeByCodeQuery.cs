using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ecommerce.Application.SaleCodes.Dto;
using Ecommerce.Domain;
using Ecommerce.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Application.SaleCodes
{
    internal class GetSaleCodeByCodeHandler : IRequestHandler<GetSaleCodeByCodeQuery, SaleCodeDto>
    {
        private readonly MainDbContext _mainDbContext;
        public GetSaleCodeByCodeHandler(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public async Task<SaleCodeDto> Handle(GetSaleCodeByCodeQuery request, CancellationToken cancellationToken)
        {
            var saleCode = await _mainDbContext.SaleCodes.Select(x => new SaleCodeDto
            {
                Code = x.Code,
                Percent = x.Percent,
                MaxPrice = x.MaxPrice,
                ValidUntil = x.ValidUntil
            }).FirstOrDefaultAsync(v => v.Code == request.Code, cancellationToken);
            if (saleCode is null)
            {
                throw new CoreException("Mã giảm giá không tồn tại");
            }
            return saleCode;
        }
    }
    public record GetSaleCodeByCodeQuery(string Code) : IRequest<SaleCodeDto>;
}