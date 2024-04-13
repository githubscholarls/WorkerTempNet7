using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WT.Trigger.Domain.Options
{
    public class RConsumerConfig
    {
        public string BootstrapServers { get; set; }
        public string GroupId { get; set; }
    }
}
