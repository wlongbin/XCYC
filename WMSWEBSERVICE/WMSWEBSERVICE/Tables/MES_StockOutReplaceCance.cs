using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMSWEBSERVICE.Tables
{
    /// <summary>
    /// 工单取消申请对象
    /// </summary>
    public class MES_StockOutReplaceCance
    {
        [DBField("WO_NO", EnumDBFieldUsage.PrimaryKey, "工单号", "CHAR", "50", "必传")]
        public string WO_NO { get; set; }
        [DBField("WO_STATUS", EnumDBFieldUsage.None, "工单状态", "CHAR", "10", "状态为14时,为撤销状态")]
        public string WO_STATUS { get; set; }
        [DBField("CANCEL_DATE_TIME", EnumDBFieldUsage.None, "取消时间", "CHAR", "10", "必传")]
        public string CANCEL_DATE_TIME { get; set; }
    }
}