using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMSWEBSERVICE.Tables
{
    /// <summary>
    /// 投料开始对象
    /// </summary>
    public class MES_OpenBegin
    {
        [DBField("WO_NO", EnumDBFieldUsage.PrimaryKey, "工单号", "CHAR", "50", "必传")]
        public string WO_NO { get; set; }
        [DBField("WO_STATUS", EnumDBFieldUsage.None, "工单状态", "CHAR", "10", "必传(2表示开始)")]
        public string WO_STATUS { get; set; }
        [DBField("START_DATE_TIME", EnumDBFieldUsage.None, "工单开始时间", "DATETIME", "19", "必传")]
        public string START_DATE_TIME { get; set; }
    }
}