using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMSWEBSERVICE.Tables
{
    /// <summary>
    /// 备料入库完成主表对象
    /// </summary>
    public class MES_MoveStockFinishM
    {
        [DBField("RAW_MOVE_STOCK_ORDER_NO", EnumDBFieldUsage.PrimaryKey, "原料移库单备料单据号", "CHAR", "30", "必传(复合主键)")]
        public string RAW_MOVE_STOCK_ORDER_NO { get; set; }
        [DBField("PLAN_BATCH_NO", EnumDBFieldUsage.PrimaryKey , "备料计划批次号", "CHAR", "30", "必传(复合主键)")]
        public string PLAN_BATCH_NO { get; set; }
        [DBField("IN_STOCK_TYPE", EnumDBFieldUsage.None, "入库类型", "CHAR", "10", "必传(1:批次入库,2:等级入库)")]
        public string IN_STOCK_TYPE { get; set; }
        [DBField("IN_STOCK_DATETIME", EnumDBFieldUsage.None, "入库时间", "DATETIME", "19", "必传")]
        public string IN_STOCK_DATETIME { get; set; }
         [DBField("IS_FINISHED_FLAG", EnumDBFieldUsage.None, "是否按照原料备料移库单完成", "CHAR", "10", "必传")]
        public string IS_FINISHED_FLAG { get; set; }

    }
}