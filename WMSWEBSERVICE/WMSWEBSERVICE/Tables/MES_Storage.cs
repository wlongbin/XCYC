using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMSWEBSERVICE.Tables
{
    /// <summary>
    /// 库存对象
    /// </summary>
    public class MES_Storage
    {
        [DBField("TOBACCO_BOX_BAR_CODE", EnumDBFieldUsage.None, "烟叶箱条码", "CHAR", "100", "必传")]
        public string TOBACCO_BOX_BAR_CODE { get; set; }
        [DBField("RAW_MAT_CD", EnumDBFieldUsage.None, "原料编码", "CHAR", "50", "必传(烟叶编码)")]
        public string RAW_MAT_CD { get; set; }
        [DBField("MATERIAL_STORAGE", EnumDBFieldUsage.None, "原料库存", "NUMBER", "12.4", "必传(单位为公斤)")]
        public string MATERIAL_STORAGE { get; set; }
        [DBField("LEAF_STANDARD", EnumDBFieldUsage.None, "烟包规格", "CHAR", "20", "必传")]
        public string LEAF_STANDARD { get; set; }
        [DBField("PURCHASE_BATCH", EnumDBFieldUsage.None, "采购批次", "CHAR", "20", "必传")]
        public string PURCHASE_BATCH { get; set; }
        [DBField("CHECK_BATCH_NO", EnumDBFieldUsage.None, "检验批次", "CHAR", "50", "必传")]
        public string CHECK_BATCH_NO { get; set; }
        [DBField("POSITION_TYPE", EnumDBFieldUsage.None, "位置类型", "NUMBER", "1", "(1表示立库货位；2表示在途；3表示在缓存线,4表示在地面缓存库)")]
        public string POSITION_TYPE { get; set; }
        [DBField("POSITION", EnumDBFieldUsage.None, "所在位置(货位号或缓存线)", "CHAR", "50", "必传")]
        public string POSITION { get; set; }

    }
}