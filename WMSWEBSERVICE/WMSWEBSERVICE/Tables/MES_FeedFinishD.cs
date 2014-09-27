using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMSWEBSERVICE.Tables
{
    /// <summary>
    /// 补料入库完成从表对象
    /// </summary>
    public class MES_FeedFinishD
    {
        [DBField("FEED_REPLACE_APPLY_BILL_NO", EnumDBFieldUsage.PrimaryKey, "补料替换申请单据号", "CHAR", "50", "必传")]
        public string FEED_REPLACE_APPLY_BILL_NO { get; set; }
        [DBField("TROUBLE_MATERIAL_SMOKEBOX_CODE", EnumDBFieldUsage.None , "问题原料烟箱条码", "CHAR", "50", "必传")]
        public string TROUBLE_MATERIAL_SMOKEBOX_CODE { get; set; }
        [DBField("TROUBLE_MATERIAL_CD", EnumDBFieldUsage.None, "问题原料编码", "CHAR", "50", "必传")]
        public string TROUBLE_MATERIAL_CD { get; set; }
        [DBField("TROUBLE_MATERIAL_AMOUNT_KG", EnumDBFieldUsage.None, "问题原料公斤数", "NUMBER", "10.4", "必传")]
        public string TROUBLE_MATERIAL_AMOUNT_KG { get; set; }
        [DBField("MATERIAL_FEED_OUT_BILL_NO", EnumDBFieldUsage.None, "原料补料出库单据号", "CHAR", "50", "必传")]
        public string MATERIAL_FEED_OUT_BILL_NO { get; set; }
        [DBField("AFTER_FEED_NEW_SMOKEBOX_CODE", EnumDBFieldUsage.None, "补料后的新烟箱条码", "CHAR", "200", "必传")]
        public string AFTER_FEED_NEW_SMOKEBOX_CODE { get; set; }
        [DBField("AFTER_FEED_NEW_MATERIAL_CD", EnumDBFieldUsage.None, "补料后的新原料编码", "CHAR", "50", "必传")]
        public string AFTER_FEED_NEW_MATERIAL_CD { get; set; }
        [DBField("FEED_AMOUNT_KG", EnumDBFieldUsage.None, "补料公斤数", "NUMBER", "10.4", "必传")]
        public string FEED_AMOUNT_KG { get; set; }

    }
}