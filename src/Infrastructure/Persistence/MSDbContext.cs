using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WT.DirectLogistics.Application.Common.Interfaces;
using WT.DirectLogistics.Domain.Common;
using WT.DirectLogistics.Domain.Entities;

namespace WT.DirectLogistics.Infrastructure.Persistence
{
    public class MSDbContext : DbContext, IMSDbContext
    {
        private readonly IDomainEventService _domainEventService;

        public MSDbContext(
            DbContextOptions<MSDbContext> options,
            IDomainEventService domainEventService):base(options)
        {
            _domainEventService = domainEventService;
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            await DispatchEvents();
            return result;
        }
        private async Task DispatchEvents()
        {
            while (true)
            {
                var domainEventEntity = ChangeTracker
                    .Entries<IHasDomainEvent>()
                    .Select(x => x.Entity.DomainEvents)
                    .SelectMany(x => x)
                    .FirstOrDefault(domainEvent => !domainEvent.IsPublished);
                if (domainEventEntity == null) break;

                domainEventEntity.IsPublished = true;
                await _domainEventService.Publish(domainEventEntity);
            }
        }
    }
}
