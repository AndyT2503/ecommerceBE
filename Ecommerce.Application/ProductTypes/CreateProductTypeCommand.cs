using Ecommerce.Domain;
using Ecommerce.Domain.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.ProductTypes
{
    internal class CreateProductTypeHandler : IRequestHandler<CreateProductTypeCommand, Unit>
    {
        private readonly MainDbContext _mainDbContext;
        public CreateProductTypeHandler(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public async Task<Unit> Handle(CreateProductTypeCommand request, CancellationToken cancellationToken)
        {
            var newProductType = new ProductType() { Name = request.Name, Code = request.Code };
            _mainDbContext.ProductTypes.Add(newProductType);
            await _mainDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
    public class CreateProductTypeCommand : IRequest<Unit>
    {
        public string Name { get; init; }
        public string Code { get; init; }
    }
}
