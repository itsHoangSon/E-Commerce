using BuildingBlock.BackgroundTasks;
using BuildingBlock.Exceptions;
using IdentityService.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Features.AppUsers.Commands
{
    public class PublishAppUserCommand : IRequest<int>
    {
        public PublishAppUserCommand()
        {
        }

    }
    public class PublishAppUserCommandHandler : IRequestHandler<PublishAppUserCommand, int>
    {
        private readonly IdentityDbContext _context;
        private readonly IMediator _mediator;
        private readonly IBackgroundMediatRQueue _backgroundMediatRQueue;
        public PublishAppUserCommandHandler(IdentityDbContext context, IMediator mediator, IBackgroundMediatRQueue backgroundMediatRQueue)
        {
            _context = context;
            _mediator = mediator;
            _backgroundMediatRQueue = backgroundMediatRQueue;
        }

        public async Task<int> Handle(PublishAppUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException("This method is not implemented yet.");
            //try
            //{
            //    List<long> ids = await _context.AppUser.Select(x => x.Id).ToListAsync();
            //    await _backgroundMediatRQueue.Publish(new AppUserChangedEvent(ids), cancellationToken);
            //    return ids.Count();
            //}
            //catch (Exception e)
            //{
            //    throw new CustomException(e.Message);
            //}
        }
    }
}
