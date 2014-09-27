using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMSWEBSERVICE.Tables
{
    /// <summary>
    /// 烟叶信息
    /// </summary>
    public class CMD_PRODUCT
    {
        [DBField("id", EnumDBFieldUsage.PrimaryKey, "烟叶ID", "VARCHAR", "12", "必传")]
        public string id { get; set; }
        [DBField("code", EnumDBFieldUsage.None, "烟叶编码", "VARCHAR", "50", "必传")]
        public string code { get; set; }
        [DBField("name", EnumDBFieldUsage.None, "烟叶名称", "VARCHAR", "100", "必传")]
        public string name { get; set; }
         [DBField("year", EnumDBFieldUsage.None, "年份", "CHAR", "4", "必传")]
        public string year { get; set; }
        [DBField("leaf_origin_id", EnumDBFieldUsage.None, "烟叶产地", "VARCHAR", "12", "必传")]
         public string leaf_origin_id { get; set; }
        [DBField("leaf_type_id", EnumDBFieldUsage.None, "烟叶类型", "VARCHAR", "12", "必传")]
        public string leaf_type_id { get; set; }
        [DBField("leaf_shape_id", EnumDBFieldUsage.None, "烟叶形态", "VARCHAR", "12", "必传")]
        public string leaf_shape_id { get; set; }
        [DBField("specification", EnumDBFieldUsage.None, "包装规格", "VARCHAR", "15", "必传")]
        public string specification { get; set; }
         [DBField("leaf_level_id", EnumDBFieldUsage.None, "烟叶等级", "VARCHAR", "12", "必传")]
        public string leaf_level_id { get; set; }
         [DBField("leaf_other_id", EnumDBFieldUsage.None, "其他属性", "VARCHAR", "12", "必传")]
         public string leaf_other_id { get; set; }
         [DBField("status", EnumDBFieldUsage.None, "状态", "VARCHAR", "2", "必传")]
         public string status { get; set; }

    }
}