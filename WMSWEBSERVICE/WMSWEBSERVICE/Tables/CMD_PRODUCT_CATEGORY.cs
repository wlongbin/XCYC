using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMSWEBSERVICE.Tables
{
    /// <summary>
    /// 烟叶类型
    /// </summary>
    public class CMD_PRODUCT_CATEGORY
    {
        [DBField("id", EnumDBFieldUsage.PrimaryKey, "烟叶类型ID", "VARCHAR", "12", "必传")]
        public string id { get; set; }
        [DBField("code", EnumDBFieldUsage.None, "烟叶类型代码", "VARCHAR", "3", "必传")]
        public string code { get; set; }
        [DBField("name", EnumDBFieldUsage.None, "烟叶类型名称", "VARCHAR", "50", "必传")]
        public string name { get; set; }
        [DBField("status", EnumDBFieldUsage.None, "状态", "VARCHAR", "2", "必传")]
        public string status { get; set; }
        [DBField("creator", EnumDBFieldUsage.None, "创建人", "VARCHAR", "30", "")]
        public string creator { get; set; }
        [DBField("createtime", EnumDBFieldUsage.None, "创建日期", "TIMESTAMP", "19", "")]
        public string createtime { get; set; }
        [DBField("modifier", EnumDBFieldUsage.None, "修改者", "VARCHAR", "30", "")]
        public string modifier { get; set; }
        [DBField("modifytime", EnumDBFieldUsage.None, "修改日期", "TIMESTAMP", "19", "")]
        public string modifytime { get; set; }
    }
}