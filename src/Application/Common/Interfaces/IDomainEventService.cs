using WT.Trigger.Domain.Common;
using System.Threading.Tasks;

namespace WT.Trigger.Application.Common.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
