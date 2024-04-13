using WT.Trigger.Application.Common.Interfaces;
using System;

namespace WT.Trigger.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
