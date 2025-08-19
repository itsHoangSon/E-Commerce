using BuildingBlock.Exceptions;
using BuildingBlock.Paginated;
using IdentityService.Data;
using IdentityService.Features.AppUsers.Models;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Features.AppUsers.Queries
{
    public class GetPaginatedAppUsersQuery : IRequest<PaginatedList<AppUserModel>>
    {
        public AppUserFilterModel SystemUserFilter;
        public GetPaginatedAppUsersQuery(AppUserFilterModel filter)
        {
            SystemUserFilter = filter;
        }
    }

    public class GetPaginatedAppUsersHandler : IRequestHandler<GetPaginatedAppUsersQuery, PaginatedList<AppUserModel>>
    {

        private readonly IdentityDbContext _context;
        public GetPaginatedAppUsersHandler(IdentityDbContext context)
        {
            _context = context;
        }


        public async Task<PaginatedList<AppUserModel>> Handle(GetPaginatedAppUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var filter = request.SystemUserFilter;
                var query = _context.AppUser.AsNoTracking();
                if (filter.Search != null)
                {
                    query = query.Where(x => x.Username.Contains(filter.Search)
                    || (x.DisplayName != null && x.DisplayName.Contains(filter.Search)));
                }
                if (filter.Username != null)
                {
                    query = query.Where(x => x.Username.Contains(filter.Username));
                }
                if (filter.DisplayName != null)
                {
                    query = query.Where(x => x.DisplayName != null && x.DisplayName.Contains(filter.DisplayName));
                }
                if (filter.Status != null)
                {
                    query = query.Where(x => x.Status == filter.Status);
                }
                if (filter.IsAdmin != null)
                {
                    query = query.Where(x => x.IsAdmin == filter.IsAdmin);
                }

                var total = await query.CountAsync();
                var items = await query.OrderByDescending(x => x.CreatedAt).ProjectToType<AppUserModel>().ToPagedAsync(filter, cancellationToken);
                return new PaginatedList<AppUserModel>(items, total, filter.PageIndex, filter.PageSize);

            }
            catch (Exception e)
            {
                throw new CustomException(e.Message);
            }
        }
    }

}
