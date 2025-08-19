using BuildingBlock.Extensions;
using E_Commerce.Data;
using E_Commerce.Entities;
using IdentityService.Features.AppUsers.Messages;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Features.AppUsers.Consumers
{
    public class ListAppUserChangedConsumer : IConsumer<ListAppUserChangedMessage>
    {
        readonly ILogger<ListAppUserChangedConsumer> _logger;
        private readonly ECommerceDbContext _context;

        public ListAppUserChangedConsumer(ILogger<ListAppUserChangedConsumer> logger, ECommerceDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task Consume(ConsumeContext<ListAppUserChangedMessage> context)
        {
            var message = context.Message;

            _logger.LogInformation($"Consume ListAppUserConfigChanged: {context.Message.Items.Count} items {context.Message.Items.ToJson()}");
            await UpdateAppUser(message.Items);
        }
        private async Task UpdateAppUser(List<AppUserChangedMessage> appUsers)
        {
            List<long> Ids = appUsers.Select(x => x.Id).ToList();
            var dbAppUsers = await _context.AppUser.Where(x => Ids.Contains(x.Id)).ToListAsync();
            dbAppUsers.ForEach(x => x.IsDeleted = true);

            foreach (var appUser in appUsers)
            {
                var subAppUser = dbAppUsers.Where(x => x.Id == appUser.Id).FirstOrDefault();

                if (subAppUser == null)
                {
                    subAppUser = new AppUser();
                    subAppUser.Id = appUser.Id;
                    _context.Add(subAppUser);
                }
                else
                {
                    subAppUser.IsDeleted = false;
                }
                subAppUser.Username = appUser.Username;
                subAppUser.DisplayName = appUser.DisplayName;
                subAppUser.Status = appUser.Status;
                subAppUser.IsAdmin = appUser.IsAdmin;
                subAppUser.IsDeleted = appUser.IsDeleted;
            }

            await _context.SaveChangesAsync();
        }
    }
}
