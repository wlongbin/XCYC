using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMSWEBSERVICE.Tables
{
    /// <summary>
    /// 出库工单归集从表对象
    /// </summary>
    public class MES_StockOutCollectionD
    {
        [DBField("WO_NO", EnumDBFieldUsage.PrimaryKey, "工单号", "CHAR", "30", "必传(复合主键)")]
        public string WO_NO { get; set; }
        [DBField("BAR_CODE", EnumDBFieldUsage.PrimaryKey, "烟箱条码", "CHAR", "200", "必传(复合主键)")]
        public string BAR_CODE { get; set; }
        [DBField("LEAF_NO", EnumDBFieldUsage.PrimaryKey, "原料代码", "CHAR", "50", "必传(复合主键)")]
        public string LEAF_NO { get; set; }
        [DBField("LEAF_NAME", EnumDBFieldUsage.None, "烟叶名称", "CHAR", "100", "必传")]
        public string LEAF_NAME { get; set; }
        [DBField("LEAF_AREA", EnumDBFieldUsage.None, "烟叶产地", "CHAR", "100", "必传")]
        public string LEAF_AREA { get; set; }
        [DBField("LEAF_YEAR", EnumDBFieldUsage.None, "烟叶年份", "CHAR", "4", "必传")]
        public string LEAF_YEAR { get; set; }
        [DBField("LEAF_GRADE", EnumDBFieldUsage.None, "烟叶等级", "CHAR", "10", "必传")]
        public string LEAF_GRADE { get; set; }
        [DBField("LEAF_TYPE", EnumDBFieldUsage.None, "烟叶类型", "CHAR", "10", "必传")]
        public string LEAF_TYPE { get; set; }
        [DBField("LEAF_STANDARD", EnumDBFieldUsage.None, "烟包规格", "CHAR", "20", "必传")]
        public string LEAF_STANDARD { get; set; }
        [DBField("IS_MERGE_FLAG", EnumDBFieldUsage.None, "是否合包", "CHAR", "4", "必传(1表示合包，0表示不合包)")]
        public string IS_MERGE_FLAG { get; set; }
        [DBField("AMOUNT_KG", EnumDBFieldUsage.None, "公斤数", "NUMBER", "12.4", "必传(单位：kg)")]
        public string AMOUNT_KG { get; set; }
        [DBField("ACTUAL_OUT_ORDER", EnumDBFieldUsage.None, "实际出库顺序", "INTEGER", " ", "必传")]
        public string ACTUAL_OUT_ORDER { get; set; }
        [DBField("PURCHASE_BATCH", EnumDBFieldUsage.None, "采购批次号", "CHAR", "20", "必传")]
        public string PURCHASE_BATCH { get; set; }
        [DBField("IN_FACTORY_CHECK_BATCH_NO", EnumDBFieldUsage.None, "原料入厂检验批次", "CHAR", "28", "必传")]
        public string IN_FACTORY_CHECK_BATCH_NO { get; set; }

    }
}