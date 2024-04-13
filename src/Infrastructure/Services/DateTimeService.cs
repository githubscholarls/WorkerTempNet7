using WT.DirectLogistics.Application.Common.Interfaces;
using System;

namespace WT.DirectLogistics.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
