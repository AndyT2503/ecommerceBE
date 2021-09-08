using Ecommerce.Application.Auth.Dto;
using Ecommerce.Domain;
using Ecommerce.Infrastructure.Common;
using Ecommerce.Infrastructure.LinQ;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.Auth
{
    internal class GetUsersHandler : IRequestHandler<GetUsersQuery, PagingModel<UserDto>>
    {
        private readonly MainDbContext _mainDbContext;
        public GetUsersHandler(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public async Task<PagingModel<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var query = _mainDbContext.Users.AsNoTracking().WhereIf(!string.IsNullOrEmpty(request.Username), x => EF.Functions.ILike(x.Username, $"%{request.Username}%"));
            var users = await query.Select(x => new UserDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Role = x.Role,
                Username = x.Username
            }).Page(request.PageIndex, request.PageSize).ToListAsync(cancellationToken);
            var totalCount = await query.CountAsync(cancellationToken);
            return new PagingModel<UserDto>(users, totalCount, request.PageIndex, request.PageSize );
        }
    }
    public class GetUsersQuery: IRequest<PagingModel<UserDto>>
    {
        public int PageIndex { get; init; }
        public int PageSize { get; init; }
        public string Username { get; init; }
    }
}
