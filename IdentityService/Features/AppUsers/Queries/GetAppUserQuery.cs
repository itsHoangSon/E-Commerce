using BuildingBlock.Exceptions;
using IdentityService.Data;
using IdentityService.Features.AppUsers.Models;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Features.AppUsers.Queries
{
    public class GetAppUserQuery : IRequest<AppUserModel>
    {
        public GetAppUserQuery()
        {

        }
        public GetAppUserQuery(long appUserId)
        {
            AppUserId = appUserId;
        }
        public long AppUserId { get; set; }
    }
    public class GetAppUserQueryHandler : IRequestHandler<GetAppUserQuery, AppUserModel>
    {

        private readonly IdentityDbContext _context;
        private readonly IMediator _mediator;
        private readonly StaticParams _staticParams;
        public GetAppUserQueryHandler(IdentityDbContext context, IMediator mediator, IOptionsMonitor<StaticParams> staticParams)
        {
            _context = context;
            _mediator = mediator;
            _staticParams = staticParams.CurrentValue;
        }

        public async Task<AppUserModel> Handle(GetAppUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var appUser = await _context.AppUser.Where(m => m.Id == request.AppUserId).AsNoTracking()
                    .ProjectToType<AppUserModel>().FirstOrDefaultAsync();
                if (appUser == null)
                {
                    appUser = new AppUserModel();
                    appUser.AddError(nameof(appUser.Id), $"AppUserId {appUser.Id} không tồn tại");
                }

                //var appUser = new AppUserModel
                //{
                //    Id = 100,
                //    Username = "name",
                //    DisplayName = "named",
                //};
                //appUser.AddError(nameof(appUser.Id), "ádfsdf");

                return appUser;
            }
            catch (Exception e)
            {
                throw new CustomException(e.Message);
            }
        }
    }
}
