using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WT.Trigger.Application.Common.Models
{
    public class SQLSource
    {
        public string version { get; set; }
        public string connector { get; set; }
        public string name { get; set; }
        public Nullable<long> ts_ms { get; set; }
        public string snapshot { get; set; }
        public string db { get; set; }
        public string sequence { get; set; }
        public string schema { get; set; }
        public string table { get; set; }
        public string change_lsn { get; set; }
        public string commit_lsn { get; set; }
        public Nullable<int> event_serial_no { get; set; }
    }
}
