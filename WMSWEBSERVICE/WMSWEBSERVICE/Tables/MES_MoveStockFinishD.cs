using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMSWEBSERVICE.Tables
{
    /// <summary>
    /// 备料入库完成从表对象
    /// </summary>
    public class MES_MoveStockFinishD
    {
        [DBField("PLAN_BATCH_NO", EnumDBFieldUsage.PrimaryKey, "备料计划批次号", "CHAR", "20", "必传(复合主键)")]
        public string PLAN_BATCH_NO { get; set; }
        [DBField("BOX_BAR_CODE", EnumDBFieldUsage.None , "烟箱条码", "CHAR", "200", "必传(复合主键)")]
        public string BOX_BAR_CODE { get; set; }
        [DBField("RAW_MAT_CD", EnumDBFieldUsage.PrimaryKey , "原料代码", "CHAR", "30", "必传(复合主键)")]
        public string RAW_MAT_CD { get; set; }
        [DBField("AMOUNT_KG", EnumDBFieldUsage.None, "原料公斤数", "NUMBER", "12.4", "必传(单位：kg)")]
        public string AMOUNT_KG { get; set; }
        [DBField("LEAF_STANDARD", EnumDBFieldUsage.None, "烟包规格", "CHAR", "20", "必传")]
        public string LEAF_STANDARD { get; set; }
        [DBField("PURCHASE_BATCH", EnumDBFieldUsage.None, "采购批次", "CHAR", "20", "必传")]
        public string PURCHASE_BATCH { get; set; }
        [DBField("IN_FACTORY_CHECK_BATCH_NO", EnumDBFieldUsage.None, "入厂检验批次", "CHAR", "28", "必传")]
        public string IN_FACTORY_CHECK_BATCH_NO { get; set; }

    }
}