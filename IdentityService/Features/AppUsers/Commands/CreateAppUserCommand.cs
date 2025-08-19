using BuildingBlock.BackgroundTasks;
using BuildingBlock.Exceptions;
using IdentityService.Data;
using IdentityService.Entities;
using IdentityService.Features.AppUsers.Events;
using IdentityService.Features.AppUsers.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Features.AppUsers.Commands
{
    public class CreateAppUserCommand : IRequest<AppUserModel>
    {
        public AppUserModel Model { get; set; }
        public CreateAppUserCommand(AppUserModel model)
        {
            Model = model;
        }

    }
    public class CreateAppUserCommandHandler : IRequestHandler<CreateAppUserCommand, AppUserModel>
    {
        private readonly IdentityDbContext _context;
        private readonly IMediator _mediator;
        private readonly IBackgroundMediatRQueue _backgroundMediatRQueue;
        public CreateAppUserCommandHandler(IdentityDbContext context, IMediator mediator, IBackgroundMediatRQueue backgroundMediatRQueue)
        {
            _context = context;
            _mediator = mediator;
            _backgroundMediatRQueue = backgroundMediatRQueue;
        }

        public async Task<AppUserModel> Handle(CreateAppUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var model = request.Model;

                if (!await Validate(model, cancellationToken))
                {
                    return model;
                }

                AppUser appUser = new AppUser(model.Password, model.Username);
                appUser.DisplayName = model.DisplayName;
                appUser.Description = model.Description;
                appUser.IsAdmin = model.IsAdmin;
                appUser.Status = model.Status;

                await _context.AppUser.AddAsync(appUser);
                await _context.SaveChangesAsync(cancellationToken);
                model.Id = appUser.Id;
                await _backgroundMediatRQueue.Publish(new AppUserChangedEvent(appUser.Id), cancellationToken);
                return model;
            }
            catch (Exception e)
            {
                throw new CustomException(e.Message);
            }
        }

        private async Task<bool> Validate(AppUserModel model, CancellationToken cancellationToken)
        {
            await model.CheckExistedUsername(_context);

            model.CheckValidPassword();

            return model.IsValidated;
        }
    }
}
