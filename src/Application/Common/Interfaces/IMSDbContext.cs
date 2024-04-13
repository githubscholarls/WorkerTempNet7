using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using WT.Trigger.Domain.Entities;

namespace WT.Trigger.Application.Common.Interfaces
{
    public interface IMSDbContext
    {

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        EntityEntry Entry(object entity);
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        int SaveChanges();
        DbSet<Com> com { get; set; }                                                            
        DatabaseFacade Database { get; }
    }
}
