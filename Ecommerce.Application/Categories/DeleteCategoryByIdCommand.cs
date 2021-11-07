using Ecommerce.Domain;
using Ecommerce.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.Categories
{
    internal class DeleteCategoryByIdHandler : IRequestHandler<DeleteCategoryByIdCommand, Unit>
    {
        private readonly MainDbContext _mainDbContext;
        public DeleteCategoryByIdHandler(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public async Task<Unit> Handle(DeleteCategoryByIdCommand request, CancellationToken cancellationToken)
        {
            var category = await _mainDbContext.Categories.FirstOrDefaultAsync(x => x.Id == request.id, cancellationToken);
            if (category is null)
            {
                throw new CoreException("Mẫu mã không tồn tại");
            }
            _mainDbContext.Categories.Remove(category);
            await _mainDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
    public record DeleteCategoryByIdCommand(Guid id):IRequest<Unit>;
}
