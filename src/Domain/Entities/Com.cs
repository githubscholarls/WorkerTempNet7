using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WT.Trigger.Domain.Entities
{
    public class Com
    {
        public int id { get; set; }
        public string com_name { get; set; }
        public string com_kind { get; set; }
        public string zhizhao { get; set; }
        public string manager { get; set; }
        public DateTime? found_time { get; set; }
        public string jianjie { get; set; }
        public string jingying { get; set; }
        public string pic { get; set; }
        public string logo { get; set; }
        public string contact { get; set; }
        public string pro { get; set; }
        public string city { get; set; }
        public string county { get; set; }
        public string email { get; set; }
        public string tel { get; set; }
        public string cell { get; set; }
        public string fax { get; set; }
        public string address { get; set; }
        public string postcode { get; set; }
        public string website { get; set; }
        public string tousutel { get; set; }
        public string danjutel { get; set; }
        public string huokuan { get; set; }
        public string hyname { get; set; }
        public int cust_id { get; set; }
        public DateTime? time { get; set; }
        public int? cishu { get; set; }
        public string qq { get; set; }
        public string lng { get; set; }
        public string lat { get; set; }
        public string servicetype { get; set; }
        public string yuangong { get; set; }
        public int? billingcycle { get; set; }
        public string paymentway { get; set; }
        public int? fee { get; set; }
        public int? isinsurance { get; set; }
        public string inpark { get; set; }
        public int? checkbill_start { get; set; }
        public int? checkbill_end { get; set; }
        public int? pay_start { get; set; }
        public int? pay_end { get; set; }
        public int? pay_type { get; set; }
        public string address_remark { get; set; }
        public int? add_parkid { get; set; }
        public string add_parkname { get; set; }
    }
}
