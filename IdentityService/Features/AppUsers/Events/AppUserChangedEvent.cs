using BuildingBlock.Extensions;
using BuildingBlock.Logging;
using IdentityService.Data;
using IdentityService.Features.AppUsers.Messages;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Features.AppUsers.Events
{
    public class AppUserChangedEvent : INotification
    {
        public AppUserChangedEvent()
        {

        }
        public AppUserChangedEvent(long id)
        {
            Ids = new List<long>() { id };

        }
        public AppUserChangedEvent(List<long> ids)
        {
            Ids = ids;
        }
        public List<long> Ids { get; set; } = new List<long>();
    }

    public class AppUserChangedEventHandler : INotificationHandler<AppUserChangedEvent>
    {
        private readonly IdentityDbContext _context;
        private readonly IAppLogger<AppUserChangedEventHandler> _logger;
        readonly IBus _bus;
        public AppUserChangedEventHandler(IdentityDbContext context, IAppLogger<AppUserChangedEventHandler> logger, IBus bus)
        {
            _context = context;
            _logger = logger;
            _bus = bus;
        }

        public async Task Handle(AppUserChangedEvent notification, CancellationToken cancellationToken)
        {
            //call messagequeue to sync data
            _logger.LogInformation($"AppUserChangedEventHandler {notification.Ids.Join()}");
            var appUsers = await _context.AppUser.Where(m => notification.Ids.Contains(m.Id)).AsNoTracking().ToListAsync(cancellationToken);

            var items = new List<AppUserChangedMessage>();
            foreach (var appUser in appUsers)
            {
                items.Add(new AppUserChangedMessage(appUser.Id, appUser.Username, appUser.DisplayName, appUser.Status, appUser.IsAdmin, appUser.IsDeleted));
            }

            await _bus.Publish(new ListAppUserChangedMessage(items), cancellationToken);

        }
    }
}
