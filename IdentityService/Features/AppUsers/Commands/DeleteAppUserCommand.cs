using BuildingBlock.BackgroundTasks;
using BuildingBlock.Exceptions;
using IdentityService.Data;
using IdentityService.Features.AppUsers.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Features.AppUsers.Commands
{
    public class DeleteAppUserCommand : IRequest<long>
    {
        public DeleteAppUserCommand(long id)
        {
            Ids = new List<long> { id };
        }
        public DeleteAppUserCommand(List<long> ids)
        {
            Ids = ids;
        }
        public List<long> Ids { get; set; }
    }

    public class DeleteAppUserCommandHandler : IRequestHandler<DeleteAppUserCommand, long>
    {
        private readonly IdentityDbContext _context;
        private readonly IBackgroundMediatRQueue _backgroundMediatRQueue;
        public DeleteAppUserCommandHandler(IdentityDbContext context, IBackgroundMediatRQueue backgroundMediatRQueue)
        {
            _context = context;
            _backgroundMediatRQueue = backgroundMediatRQueue;
        }

        public async Task<long> Handle(DeleteAppUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var appUsers = await _context.AppUser.Where(m => request.Ids.Contains(m.Id)).ToListAsync();
                var Ids = appUsers.Select(m => m.Id).ToList();
                _context.AppUser.RemoveRange(appUsers);
                await _context.SaveChangesAsync();
                await _backgroundMediatRQueue.Publish(new AppUserChangedEvent(Ids), cancellationToken);
                return appUsers.Count;
            }
            catch (Exception e)
            {
                throw new CustomException(e.Message);
            }
        }
    }
}
