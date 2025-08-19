using BuildingBlock.BackgroundTasks;
using BuildingBlock.Exceptions;
using IdentityService.Data;
using IdentityService.Features.AppUsers.Events;
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
    public class UpdateAppUserCommand : IRequest<AppUserModel>
    {
        public AppUserModel Model { get; set; }
        public UpdateAppUserCommand(AppUserModel model)
        {
            Model = model;
        }
    }

    public class UpdateAppUserCommandHandler : IRequestHandler<UpdateAppUserCommand, AppUserModel>
    {
        private readonly IdentityDbContext _context;
        private readonly IBackgroundMediatRQueue _backgroundMediatRQueue;
        public UpdateAppUserCommandHandler(IdentityDbContext context, IBackgroundMediatRQueue backgroundMediatRQueue)
        {
            _context = context;
            _backgroundMediatRQueue = backgroundMediatRQueue;
        }

        public async Task<AppUserModel> Handle(UpdateAppUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var model = request.Model;
                await Validate(model, cancellationToken);
                if (!model.IsValidated)
                {
                    return model;
                }
                var dbEntity = await _context.AppUser.FirstOrDefaultAsync(x => x.Id == model.Id);

                dbEntity.Username = model.Username;
                dbEntity.DisplayName = model.DisplayName;
                dbEntity.Description = model.Description;
                dbEntity.Status = model.Status;
                dbEntity.IsAdmin = model.IsAdmin;
                await _context.SaveChangesAsync(cancellationToken);
                await _backgroundMediatRQueue.Publish(new AppUserChangedEvent(dbEntity.Id), cancellationToken);

                return model;
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

            await model.CheckExistedUsername(_context);
        }
    }
}
