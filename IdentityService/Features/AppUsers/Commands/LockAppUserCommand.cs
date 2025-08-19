using BuildingBlock.Exceptions;
using IdentityService.Data;
using IdentityService.Entities;
using IdentityService.Enums;
using IdentityService.Features.AppUsers.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Features.AppUsers.Commands
{
    public class LockAppUserCommand : IRequest<AppUserModel>
    {
        public long AppUserId { get; set; }
        public LockAppUserCommand(long appUserId)
        {
            AppUserId = appUserId;
        }
    }

    public class LockAppUserCommandHandler : IRequestHandler<LockAppUserCommand, AppUserModel>
    {
        private readonly IdentityDbContext _context;
        public LockAppUserCommandHandler(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task<AppUserModel> Handle(LockAppUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbEntity = await _context.AppUser.FirstOrDefaultAsync(m => m.Id == request.AppUserId && m.Status == StatusEnum.Active, cancellationToken);
                AppUserModel response = new AppUserModel();
                if (dbEntity == null)
                {
                    response.AddError(nameof(AppUser.Id), $"Không tìm thấy Id {request.AppUserId}");
                    return response;
                }

                dbEntity.Status = StatusEnum.InActive;
                await _context.SaveChangesAsync();

                return response;
            }
            catch (Exception e)
            {
                throw new CustomException(e.Message);
            }
        }

    }

}
