using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
   public class MES_PREPARER_PLAN
    {
       public DateTime PLAN_DATE { get; set; }
       public DateTime LATE_PREPARE_DATETIME { get; set; }
       public string MAT_CD { get; set; }
       public decimal BATCHES { get; set; }
       public string SENDER { get; set; }
       public DateTime SEND_DATETIME { get; set; }
       public Nullable<System.DateTime> REC_DT { get; set; }
        public string RECEIVER { get; set; }
        public string MSGID { get; set; }
    }
}
