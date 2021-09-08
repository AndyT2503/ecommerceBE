using Ecommerce.Domain;
using Ecommerce.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.ProductTypes
{
    internal class UpdateProductTypeHandler : IRequestHandler<UpdateProductTypeCommand, Unit>
    {
        private readonly MainDbContext _mainDbContext;
        public UpdateProductTypeHandler(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public async Task<Unit> Handle(UpdateProductTypeCommand request, CancellationToken cancellationToken)
        {
            var productType = await _mainDbContext.ProductTypes.FirstOrDefaultAsync(x => x.Id == request.Id);
            if(productType is null)
            {
                throw new CoreException("Item Not Found");
            }
            productType.Name = request.Name;
            productType.Code = request.Code;
            await _mainDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
    public class UpdateProductTypeCommand : IRequest<Unit>
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Code { get; init; }
    }
}
