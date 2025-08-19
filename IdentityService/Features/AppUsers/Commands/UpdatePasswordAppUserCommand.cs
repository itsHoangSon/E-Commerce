using BuildingBlock.Exceptions;
using BuildingBlock.Web;
using IdentityService.Data;
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
    public class UpdatePasswordAppUserCommand : IRequest<AppUserModel>
    {
        public AppUserModel Model { get; set; }
        public UpdatePasswordAppUserCommand(AppUserModel model)
        {
            Model = model;
        }
    }

    public class UpdatePasswordAppUserCommandHandler : IRequestHandler<UpdatePasswordAppUserCommand, AppUserModel>
    {
        private readonly IdentityDbContext _context;
        private readonly ICurrentUserProvider _currentUserProvider;
        public UpdatePasswordAppUserCommandHandler(IdentityDbContext context, ICurrentUserProvider currentUserProvider)
        {
            _context = context;
            _currentUserProvider = currentUserProvider;
        }

        public async Task<AppUserModel> Handle(UpdatePasswordAppUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var appUser = request.Model;
                if (!_currentUserProvider.IsAdmin)
                {
                    appUser.AddError(nameof(appUser.IsAdmin), "Not Permission");
                    return appUser;
                }
                var model = request.Model;

                await Validate(model, cancellationToken);
                if (!model.IsValidated)
                {
                    return model;
                }

                var oldEntity = await _context.AppUser.FirstOrDefaultAsync(x => x.Id == model.Id);
                oldEntity.GenNewPassword(model.Password);
                await _context.SaveChangesAsync(cancellationToken);

                return appUser;
            }
            catch (Exception e)
            {
                throw new CustomException(e.Message);
            }
        }

        private async Task Validate(AppUserModel model, CancellationToken cancellationToken)
        {
            var dbEntity = await _context.AppUser.AnyAsync(m => m.Id == model.Id, cancellationToken);
            if (!dbEntity)
            {
                model.AddError(nameof(model.Id), $"AppUserId {model.Id} không tồn tại");
                return;
            }

            model.CheckValidPassword();
            ;
        }
    }

}
