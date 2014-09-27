using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMSWEBSERVICE.Tables
{
    /// <summary>
    /// 补料入库完成主表对象
    /// </summary>
    public class MES_FeedFinishM
    {
        [DBField("FEED_IN_Finish_DATE", EnumDBFieldUsage.None, "补料入库完成日期", "DATETIME", "19", "必传")]
        public string FEED_IN_Finish_DATE { get; set; }
        [DBField("MATERIAL_FEED_OUT_BILL_NO", EnumDBFieldUsage.None, "原料补料出库单据号", "CHAR", "50", "必传")]
        public string MATERIAL_FEED_OUT_BILL_NO { get; set; }
        [DBField("FEED_BILL_OUT_DATE", EnumDBFieldUsage.None, "补料单据出库日期", "DATETIME", "19", "必传")]
        public string FEED_BILL_OUT_DATE { get; set; }
        [DBField("FEED_BILL_MAKERS", EnumDBFieldUsage.None, "补料单据制定人", "CHAR", "50", "必传")]
        public string FEED_BILL_MAKERS { get; set; }
        [DBField("FEED_REPLACE_APPLY_BILL_NO", EnumDBFieldUsage.None, "补料替换申请单据号", "CHAR", "50", "必传")]
        public string FEED_REPLACE_APPLY_BILL_NO { get; set; }
        [DBField("FEED_REPLACE_APPLY_APPLICANT", EnumDBFieldUsage.None, "补料替换申请人", "CHAR", "50", "必传")]
        public string FEED_REPLACE_APPLY_APPLICANT { get; set; }
        [DBField("FEED_REPLACE_APPLY_DATE", EnumDBFieldUsage.None, "补料替换申请时间", "DATETIME", "19", "必传")]
        public string FEED_REPLACE_APPLY_DATE { get; set; }
        [DBField("FEED_CAUSE", EnumDBFieldUsage.None, "补料原因", "CHAR", "500", "必传(虫咬、霉变、异型包、其它)")]
        public string FEED_CAUSE { get; set; }
        [DBField("REMARK", EnumDBFieldUsage.None, "备注", "CHAR", "500", "非必传")]
        public string REMARK { get; set; }

    }
}