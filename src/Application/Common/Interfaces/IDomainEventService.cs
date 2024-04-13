using WT.DirectLogistics.Domain.Common;
using System.Threading.Tasks;

namespace WT.DirectLogistics.Application.Common.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
