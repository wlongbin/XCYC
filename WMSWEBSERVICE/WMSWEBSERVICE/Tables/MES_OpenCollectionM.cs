using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMSWEBSERVICE.Tables
{
    /// <summary>
    /// 开包投料信息归集主表对象
    /// </summary>
    public class MES_OpenCollectionM
    {
        [DBField("WO_NO", EnumDBFieldUsage.PrimaryKey, "片烟出库工单号", "CHAR", "30", "必传")]
        public string WO_NO { get; set; }
        [DBField("MAT_CD", EnumDBFieldUsage.None, "牌号编码", "CHAR", "30", "必传")]
        public string MAT_CD { get; set; }
        [DBField("BOM_VER_CD", EnumDBFieldUsage.None, "配方版本号", "CHAR", "30", "必传")]
        public string BOM_VER_CD { get; set; }
        [DBField("MODULE_NO", EnumDBFieldUsage.None, "模块号", "CHAR", "10", "必传")]
        public string MODULE_NO { get; set; }
        [DBField("OUTBOUND_START_TIME", EnumDBFieldUsage.None, "出库开始时间", "DATETIME", "19", "必传")]
        public string OUTBOUND_START_TIME { get; set; }
        [DBField("OUTBOUND_END_TIME", EnumDBFieldUsage.None, "出库结束时间", "DATETIME", "19", "必传")]
        public string OUTBOUND_END_TIME { get; set; }
    }
}