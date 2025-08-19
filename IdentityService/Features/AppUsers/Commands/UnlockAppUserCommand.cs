using BuildingBlock.Exceptions;
using IdentityService.Data;
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
    public class UnlockAppUserCommand : IRequest<AppUserModel>
    {
        public long AppUserId { get; set; }
        public UnlockAppUserCommand(long appUserId)
        {
            AppUserId = appUserId;
        }
    }

    public class UnlockAppUserCommandHandler : IRequestHandler<UnlockAppUserCommand, AppUserModel>
    {
        private readonly IdentityDbContext _context;
        public UnlockAppUserCommandHandler(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task<AppUserModel> Handle(UnlockAppUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbEntity = await _context.AppUser.FirstOrDefaultAsync(m => m.Id == request.AppUserId && m.Status == StatusEnum.InActive, cancellationToken);
                AppUserModel response = new AppUserModel();
                if (dbEntity == null)
                {
                    response.AddError(nameof(AppUserModel.Id), $"UserId {request.AppUserId} không tồn tại");
                    return response;
                }

                dbEntity.Status = StatusEnum.Active;
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
