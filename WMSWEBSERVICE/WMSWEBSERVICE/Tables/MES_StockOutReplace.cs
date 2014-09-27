using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMSWEBSERVICE.Tables
{
    /// <summary>
    /// 出库工单申请对象
    /// </summary>
    public class MES_StockOutReplace
    {
        [DBField("TECH_CD", EnumDBFieldUsage.PrimaryKey , "工艺段编码", "CHAR", "40", "必传")]
        public string TECH_CD { get; set; }
    }
}