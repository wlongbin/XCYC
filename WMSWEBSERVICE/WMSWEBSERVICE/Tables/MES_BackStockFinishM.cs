using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMSWEBSERVICE.Tables
{
    /// <summary>
    /// 退料出库完成主表对象
    /// </summary>
    public class MES_BackStockFinishM
    {
        [DBField("BACK_STOCK_ORDER_NO", EnumDBFieldUsage.PrimaryKey, "退库单据号", "CHAR", "30", "必传(复合主键)")]
        public string BACK_STOCK_ORDER_NO { get; set; }
        [DBField("PLAN_BATCH_NO", EnumDBFieldUsage.PrimaryKey, "备料计划批次号", "CHAR", "20", "必传(复合主键)")]
        public string PLAN_BATCH_NO { get; set; }
        [DBField("OUT_STOCK_TYPE", EnumDBFieldUsage.None, "出库类型", "CHAR", "10", "必传(1:批次出库,2:等级出库)")]
        public string OUT_STOCK_TYPE { get; set; }
        [DBField("IN_STOCK_DATETIME", EnumDBFieldUsage.None, "出库时间", "DATETIME", "19", "必传")]
        public string IN_STOCK_DATETIME { get; set; }


    }
}