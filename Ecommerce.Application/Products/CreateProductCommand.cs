using Ecommerce.Application.Products.Dto;
using Ecommerce.Domain;
using Ecommerce.Domain.Helper;
using Ecommerce.Domain.Model;
using Ecommerce.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.Products
{
    internal class CreateProductHandler : IRequestHandler<CreateProductCommand, Unit>
    {
        private readonly MainDbContext _mainDbContext;
        public CreateProductHandler(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public async Task<Unit> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var validateMess = await ValidateSlug(request.Name, cancellationToken);
            if (!validateMess.Equals(""))
            {
                throw new CoreException(validateMess);
            }
            var newProduct = new Product();
            newProduct.Name = request.Name;
            newProduct.Slug = StringHelper.GenerateSlug(request.Name);
            newProduct.Description = request.Description;
            newProduct.Status = request.Status;
            newProduct.SpecialFeatures = request.SpecialFeatures.ToList();
            newProduct.SupplierId = request.SupplierId;
            newProduct.ProductTypeId = request.ProductTypeId;
            newProduct.Configuration = request.Configuration;
            newProduct.OriginalPrice = request.OriginalPrice;
            newProduct.AvailableStatus = request.AvailableStatus;
            foreach (var item in request.Categories)
            {
                newProduct.Categories.Add(new Category() { Image = item.Image, Price = item.Price, Name = item.Name });
            }
            _mainDbContext.Products.Add(newProduct);
            await _mainDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
        private async Task<string> ValidateSlug(string name, CancellationToken cancellationToken)
        {
            var slug = StringHelper.GenerateSlug(name);
            var product = await _mainDbContext.Products.AsNoTracking().FirstOrDefaultAsync(v => v.Slug == slug, cancellationToken);
            if (product is not null)
            {
                return "Tên sản phẩm đã tồn tại! Vui lòng thay đổi ";
            }
            return "";
        }
    }
    public class CreateProductCommand : IRequest<Unit>
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public string Status { get; init; }
        public string AvailableStatus { get; init; }
        public decimal OriginalPrice { get; init; }
        public IEnumerable<string> SpecialFeatures { get; init; }
        public Guid SupplierId { get; init; }
        public Guid ProductTypeId { get; init; }
        public object Configuration { get; init; }
        public IEnumerable<ProductCategoryDto> Categories { get; init; }
    }
}
