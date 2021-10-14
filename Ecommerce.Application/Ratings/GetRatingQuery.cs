using Ecommerce.Application.Ratings.Dto;
using Ecommerce.Domain;
using Ecommerce.Infrastructure.Common;
using Ecommerce.Infrastructure.LinQ;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.Ratings
{
    internal class GetRatingQueryHandler : IRequestHandler<GetRatingQuery, PagingModel<RatingDto>>
    {
        private readonly MainDbContext _mainDbContext;
        public GetRatingQueryHandler(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public async Task<PagingModel<RatingDto>> Handle(GetRatingQuery request, CancellationToken cancellatitonToken)
        {
            var query = _mainDbContext.Ratings.AsNoTracking()
                .Where(x => x.Product.Slug == request.slug)
                .Select(x => new RatingDto
                {
                    Comment = x.Comment,
                    Rate = x.Rate,
                    ImageUrl = x.ImageUrl,
                    CreateAt = x.CreatedAt,
                    UserName = x.UserName
                });
            var ratings = new List<RatingDto>();
            var totalCount = await query.CountAsync(cancellatitonToken);
            if (request.dto.PageSize is not null && request.dto.PageIndex is not null)
            {
                ratings = await query.Page(request.dto.PageIndex.Value, request.dto.PageSize.Value).ToListAsync(cancellatitonToken);
            }
            else
            {
                ratings = await query.ToListAsync(cancellatitonToken);
            }

            return new PagingModel<RatingDto>(ratings, totalCount, request.dto.PageIndex.GetValueOrDefault(), request.dto.PageSize.GetValueOrDefault());
        }

        public record GetRatingQuery(string slug, GetRatingDto dto) : IRequest<PagingModel<RatingDto>> { }
        public class GetRatingDto
        {
            public int? PageSize { get; init; }
            public int? PageIndex { get; init; }
        }

    }
}
