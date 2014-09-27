using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMSWEBSERVICE.Tables
{
    /// <summary>
    /// 原料测算对象
    /// </summary>
    public class MES_MATERIAESTIMATE
    {
        [DBField("TOBACCO_MAT_CD", EnumDBFieldUsage.PrimaryKey , "烟丝或膨胀丝牌号", "CHAR", "50", "必传")]
        public string TOBACCO_MAT_CD { get; set; }
        [DBField("LACK_BATCH_AMOUNT", EnumDBFieldUsage.None, "所缺批次数", "NUMBER", "4", "必传")]
        public string LACK_BATCH_AMOUNT { get; set; }
        [DBField("MATERIAL_CD", EnumDBFieldUsage.None, "原料编码", "CHAR", "50", "必传")]
        public string MATERIAL_CD { get; set; }
        [DBField("LACK_MATERIAL_AMOUNT", EnumDBFieldUsage.None, "所缺原料数量", "NUMBER", "10.4", "必传")]
        public string LACK_MATERIAL_AMOUNT { get; set; }
    }
}