using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMSWEBSERVICE.Tables
{
    /// <summary>
    /// 上传烟叶替换记录对象
    /// </summary>
    public class MES_FeedingSend
    {
        [DBField("EXCEPT_TOBACCO_BAR_CODE", EnumDBFieldUsage.PrimaryKey, "异常烟包条码", "CHAR", "200", "必传")]
        public string EXCEPT_TOBACCO_BAR_CODE { get; set; }
        [DBField("EXCEPT_TOBACCO_MATERIAL_CODE", EnumDBFieldUsage.PrimaryKey, "异常烟包原料编码", "CHAR", "50", "必传")]
        public string EXCEPT_TOBACCO_MATERIAL_CODE { get; set; }
        [DBField("NEW_TOBACCO_BAR_CODE", EnumDBFieldUsage.PrimaryKey, "新烟包条码", "CHAR", "200", "必传")]
        public string NEW_TOBACCO_BAR_CODE { get; set; }
        [DBField("NEW_TOBACCO_MATERIAL_CODE", EnumDBFieldUsage.None , "新烟包原料编码", "CHAR", "50", "必传")]
        public string NEW_TOBACCO_MATERIAL_CODE { get; set; }
        [DBField("AMOUNT_KG", EnumDBFieldUsage.None , "公斤数", "NUMBER", "12.4", "必传")]
        public string AMOUNT_KG { get; set; }
        [DBField("PLACE_DATE", EnumDBFieldUsage.None , "替换日期", "DATE", "19", "必传")]
        public string PLACE_DATE { get; set; }
    }
}