using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public partial class WMS_REPLACE_MASTER
    {
        public string BILL_NO { get; set; }
        public System.DateTime BILL_DATE { get; set; }
        public string OPERATER { get; set; }
        public Nullable<System.DateTime> OPERATE_DATE { get; set; }
        public string REASON { get; set; }
        public string STATE { get; set; }
        public string REMARK { get; set; }
        public string CHECKER { get; set; }
        public Nullable<System.DateTime> CHECK_DATE { get; set; }
        public string TRANSER { get; set; }
        public Nullable<System.DateTime> TRANS_DATE { get; set; }
    }
}
