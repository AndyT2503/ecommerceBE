using Ecommerce.Domain;
using Ecommerce.Domain.Model;
using Ecommerce.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.SaleCodes
{
    public class CreateSaleCodeCommand : IRequest<Unit>
    {
        public string Code { get; init; }
        public int Percent { get; init; }
        public decimal MaxPrice { get; init; }
        public DateTime ValidUntil { get; init; }
    }

    internal class CreateSaleCodeHandler : IRequestHandler<CreateSaleCodeCommand, Unit>
    {
        private readonly MainDbContext _mainDbContext;
        public CreateSaleCodeHandler(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public async Task<Unit> Handle(CreateSaleCodeCommand request, CancellationToken cancellationToken)
        {
            var isValidateSaleCode = await ValidateUser(request.Code, cancellationToken);
            if (!isValidateSaleCode)
            {
                throw new CoreException("Mã giảm giá đã tồn tại");
            }
            var newSaleCode = new SaleCode() { Code = request.Code, Percent = request.Percent, MaxPrice = request.MaxPrice, ValidUntil = request.ValidUntil };
            _mainDbContext.SaleCodes.Add(newSaleCode);
            await _mainDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private async Task<bool> ValidateUser(string code, CancellationToken cancellationToken)
        {
            var saleCode = await _mainDbContext.SaleCodes.AsNoTracking().FirstOrDefaultAsync(v => v.Code == code, cancellationToken);
            if (saleCode is null)
            {
                return true;
            }
            return false;
        }
    }
}
