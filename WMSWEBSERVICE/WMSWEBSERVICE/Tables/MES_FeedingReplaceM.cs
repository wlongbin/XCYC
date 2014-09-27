using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMSWEBSERVICE.Tables
{
    /// <summary>
    /// 补料替换申请主表对象
    /// </summary>
    public class MES_FeedingReplaceM
    {
        [DBField("FEED_REPLACE_APPLY_BILL_NO", EnumDBFieldUsage.PrimaryKey, "补料替换申请单据号", "CHAR", "50", "必传")]
        public string FEED_REPLACE_APPLY_BILL_NO { get; set; }
        [DBField("APPLY_DATE", EnumDBFieldUsage.None , "申请时间", "DATETIME", "19", "必传")]
        public string APPLY_DATE { get; set; }
        [DBField("APPLICANT", EnumDBFieldUsage.None, "申请人", "CHAR", "50", "必传")]
        public string APPLICANT { get; set; }
        [DBField("FEED_CAUSE", EnumDBFieldUsage.None, "补料原因", "CHAR", "500", "必传(虫咬、霉变、异型包、其它)")]
        public string FEED_CAUSE { get; set; }
        [DBField("REMARK", EnumDBFieldUsage.None, "备注", "CHAR", "500", "必传")]
        public string REMARK { get; set; }

    }
}