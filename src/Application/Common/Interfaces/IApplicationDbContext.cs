using System.Threading;
using System.Threading.Tasks;

namespace WT.DirectLogistics.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
