using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WT.DirectLogistics.Domain.Entities;

namespace WT.DirectLogistics.Application.Common.Interfaces
{
    public interface IMSDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
