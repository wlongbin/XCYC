using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMSWEBSERVICE.Tables
{
    public class MES_FORMULA_DETAIL
    {
        [DBField("BOM_VER_NO", EnumDBFieldUsage.PrimaryKey, "BOM版本号", "CHAR", "50", "必传")]
        public string BOM_VER_NO { get; set; }
        [DBField("MODULE_CD", EnumDBFieldUsage.None, "模块号", "CHAR", "20", "必传")]
        public string MODULE_CD { get; set; }
        [DBField("RAW_MAT_CD", EnumDBFieldUsage.None, "原料编码", "CHAR", "50", "必传")]
        public string RAW_MAT_CD { get; set; }
        [DBField("AMOUNT_KG", EnumDBFieldUsage.None, "公斤数", "NUMBER", "12.4", "必传")]
        public string AMOUNT_KG { get; set; }
        [DBField("ORDER_NO", EnumDBFieldUsage.None, "投料顺序", "NUMBER", "4", "必传")]
        public string ORDER_NO { get; set; }
        [DBField("REMARK", EnumDBFieldUsage.None, "备注", "CHAR", "500", "必传")]
        public string REMARK { get; set; }
    }
}