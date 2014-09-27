using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMSWEBSERVICE.Tables
{
    public class MES_FORMULA_MASTER
    {
        [DBField("BOM_VER_NO", EnumDBFieldUsage.PrimaryKey, "配方版本号", "CHAR", "50", "必传")]
        public string BOM_VER_NO { get; set; }
        [DBField("MAT_CD", EnumDBFieldUsage.None, "牌号编码", "CHAR", "50", "必传")]
        public string MAT_CD { get; set; }
        [DBField("FACTORY_CD", EnumDBFieldUsage.None, "工厂编码", "CHAR", "20", "必传")]
        public string FACTORY_CD { get; set; }
        [DBField("CHECHER", EnumDBFieldUsage.None, "审核人", "CHAR", "20", "必传")]
        public string CHECHER { get; set; }
         [DBField("CHECH_DT", EnumDBFieldUsage.None, "审核时间", "DATETIME", "19", "必传")]
        public string CHECH_DT { get; set; }
        [DBField("SENDER", EnumDBFieldUsage.None, "下发人", "CHAR", "20", "必传")]
         public string SENDER { get; set; }
        [DBField("SEND_DT", EnumDBFieldUsage.None, "下发时间", "DATETIME", "19", "必传")]
        public string SEND_DT { get; set; }
        [DBField("REMARK", EnumDBFieldUsage.None, "备注", "CHAR", "500", "必传")]
        public string REMARK { get; set; }
    }
}