using BuildingBlock.Entities;
using BuildingBlock.OS;
using BuildingBlock.Web;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.EFCore
{
    public abstract class AppDbContextBase : DbContext, IDbContext
    {
        protected readonly ICurrentUserProvider _currentUserProvider;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IPublisher _publisher;

        protected AppDbContextBase()
        {

        }
        protected AppDbContextBase(DbContextOptions options, ICurrentUserProvider currentUserProvider, IDateTimeProvider dateTimeProvider, IPublisher publisher) :
            base(options)
        {
            _currentUserProvider = currentUserProvider;
            _dateTimeProvider = dateTimeProvider;
            _publisher = publisher;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        }

        //ref: https://learn.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency#execution-strategies-and-transactions
        public Task ExecuteTransactionalAsync(CancellationToken cancellationToken = default)
        {
            var strategy = Database.CreateExecutionStrategy();
            return strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await Database.BeginTransactionAsync(cancellationToken);
                try
                {
                    await SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            var now = _dateTimeProvider.Now;
            var userId = _currentUserProvider.UserId;
            var tenantId = _currentUserProvider.TenantId;

            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = now;
                        entry.Entity.LastModifiedAt = now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedAt = now;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.LastModifiedAt = now;
                        entry.Entity.IsDeleted = true;
                        break;
                }

                if (userId > 0)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.Entity.CreatorId = userId;
                            entry.Entity.LastModifierId = userId;
                            break;

                        case EntityState.Modified:
                            entry.Entity.LastModifierId = userId;
                            break;

                        case EntityState.Deleted:
                            entry.Entity.LastModifierId = userId;
                            break;

                    }
                }
            }


            foreach (var entry in ChangeTracker.Entries<ITenantEntity>())
            {
                if (entry.Entity.TenantId == 0)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                        case EntityState.Modified:
                            entry.Entity.TenantId = tenantId;
                            break;
                    }
                }
            }

            var result = await base.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                var domainEntities = ChangeTracker.Entries<BaseEntity>().Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

                var domainEvents = domainEntities
                      .SelectMany(x => x.Entity.DomainEvents)
                      .ToList();
                domainEntities.ToList().ForEach(entity => entity.Entity.ClearDomainEvents());

                foreach (var domainEvent in domainEvents)
                {
                    await _publisher.Publish(domainEvent);
                }
            }
            return result;
        }
    }
}
